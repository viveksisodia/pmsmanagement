<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="Addnewproject.aspx.cs" Inherits="Addnewproject" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="display:none;">

                                        <asp:Button ID="btnreloadsup" runat="server" Text="changesup" OnClick="ddlclientname_SelectedIndexChanged" />
                                         <asp:Button ID="btnbindclint" runat="server" Text="changesup" OnClick="btnbindclint_Click" />
        <asp:HiddenField ID="hdnnewclient" runat="server" />
                                </div>
    <div class="row">
                                <div style="display: none">
    <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />
 <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
 <asp:Button ID="btnstateload" runat="server" Text="" OnClick="Countryid_SelectedIndexChanged" />
                                   
   
</div>
                                <script>
function countryclick()
{
debugger
//alert('1');
document.getElementById('<%=btnstateload.ClientID%>').click();

}
                                </script>
                                 <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project ID", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="ProjectID" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   <input type="text" id="ProjectID" class="form-control"  placeholder="Project ID" runat="server" >
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project Name", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectName" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="ProjectName" class="form-control"  placeholder="Project Name" runat="server">
                                </div>


                                 <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkprojectWeightage" runat="server"  />  Do You Want Same Weightage on Milestone 
                                                          </label>
                                          
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                             <label class="form-label">
                                             &nbsp;<input class="form-check-input" type="checkbox" id="checkprojecttask" runat="server" onclick="showtaskdetails(this.id)" />  Has Task  
                                                          </label>
                                          
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-10 col-lg-12" id="dvprojecttaskWeightage" style="display:none">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                             <label class="form-label">
                                             &nbsp;&nbsp;&nbsp;<input class="form-check-input" type="checkbox" id="checkprojecttaskWeightage" runat="server"   />  Do You Want Same Weightage on Task  
                                                          </label>
                                          
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-10 col-lg-12" id="dvprojectsubtask" style="display:none">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                             <label class="form-label">
                                              &nbsp;<input class="form-check-input" type="checkbox" id="checkprojectsubtask" runat="server"  onclick="showsubtaskdetails(this.id)"/>  Has Sub Task 
                                                          </label>
                                          
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-10 col-lg-12" id="dvprojectsubtaskWeightage" style="display:none" >
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                             <label class="form-label">
                                              &nbsp;&nbsp;&nbsp;<input class="form-check-input" type="checkbox" id="checkprojectsubtaskWeightage" runat="server" />  Do You Want Same Weightage on Sub Task
                                          
                                        </div>
                                    </div>
                                </div>

                                 <div class="col-xxl-12 col-lg-12" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" class="form-control">
                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="CmpOwner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      

                                </div>



                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                      

                                </div> 


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="ProjectStatus" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />

                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ProjectStatus" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectStatus" runat="server" Value="0" />
                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                
                                           



                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project Address", "2")%></label>
                                    </div>
                                    <div class="row">
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="ProjectAddress1" runat="server" class="form-control" placeholder="Address Line 1">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="ProjectAddress2" runat="server" class="form-control" placeholder="Address Line 2">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" id="City" runat="server" class="form-control" placeholder="City">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                          <%--onchcnge="countryclick();"--%>
                                           <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                          <Triggers>
                                               <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />

    
                                          </Triggers>
                                         <ContentTemplate>

                                            <cc1:DropDownListChosen ID="Countryid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Country ..." AllowSingleDeselect="true"  AutoPostBack="true" OnSelectedIndexChanged="Countryid_SelectedIndexChanged" >
                                                </cc1:DropDownListChosen>
                                             &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Countryid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                                               <asp:HiddenField ID="hdnCountryid" runat="server" Value="0" />
                                          
                                            
                                         
                                         </ContentTemplate>
                                        </asp:UpdatePanel>
                                           

                                        </div>

                                         <div class="col-xxl-7 col-lg-7">
                                           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                               <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="Countryid" EventName="SelectedIndexChanged" />
                                             
                                          </Triggers>
                                         <ContentTemplate>

                                            <cc1:DropDownListChosen ID="StateName" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="State ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnstatename" runat="server" Value="0" />
                                          
                                              <script type="text/javascript">
    Sys.Application.add_load(function () {
        $('.chzn-select').each(function () {
            var $this = $(this);
            if ($this.data('chosen')) {
                $this.chosen('destroy');
            }
            $this.chosen(); // re-initialize Chosen
        });
    });
</script>
                                         </ContentTemplate>
                                        </asp:UpdatePanel>


                                        </div>

                                        <div class="col-xxl-5 col-lg-5">
                                            <input type="text" id="PostalCode" runat="server" class="form-control" placeholder="Zip/Postal Code">
                                        </div>
                                       
                                        
                                    </div>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Client Name", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                               <asp:AsyncPostBackTrigger ControlID="btnbindclint" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                    <cc1:DropDownListChosen ID="ddlclientname" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Client ..." AllowSingleDeselect="true"  onchange="changeBillingAddresschangepopup(this.value);" >
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnclientname" runat="server" Value="0" />
                                </ContentTemplate>
                                        </asp:UpdatePanel>
                                         <a href="javascript:void(0);" id="btnaddsuplier" data-bs-toggle="modal" data-bs-target="#addnewsuplier" style="display:none;"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="+ Add Owner">+</span></a> 
                                    <div style="display:none">
                                    <input type="text" id="ClientName" runat="server" placeholder="Client Name" class="form-control">
                                        </div>
                                </div>
                                 <div class="col-xxl-12 col-lg-12">
                                           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="ddlclientname" EventName="SelectedIndexChanged" />
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnreloadsup" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnbindclint" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                            <cc1:DropDownListChosen ID="ddlsalesorder" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Sales order ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnsoid" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                        </div>

                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator4" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                             
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                             

                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budget" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actual" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Date of Award", "2")%></label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                        <input type="date" id="AwardDate" runat="server" class="form-control">

                                           <asp:HiddenField ID="hdnawarddate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                    <input type="date"  id="StartDate" runat="server" class="form-control" onchange="checkdate(this.id)">
                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="TargetDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="TargetDate" runat="server" class="form-control" onchange="checkdate(this.id)">
                                              <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                                   <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="isdrawing" runat="server" onchange="DrawingRequired();" />Drawing Required 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-xxl-6 col-lg-6" id="divaprovaldraing" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Approval of Drawing", "2")%></label>
                                    </div>
                              
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ApprovalDrawing" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="ApprovalDrawing ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">Select</asp:ListItem>
                                          <asp:ListItem  value="1">Yes</asp:ListItem>
                                          <asp:ListItem  value="2">No</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnaprovaldrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                              

                                </div>
                                <div class="col-xxl-6 col-lg-6" id="divaprovalDate" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Drawing Approval Date", "2")%></label>
                                    </div>
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="date" id="DrawingApprovalDate" runat="server" class="form-control">
                                             <asp:HiddenField ID="hdndrawingaprvldate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>  
                                
                                <div style="display: none">
                             <asp:Button ID="btnuploadimg" runat="server" Text="" OnClick="btnuploadimg_Click" />
                               </div>

                                <div class="col-xxl-12 col-lg-12" id="divuploaddrawing" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Upload drawing", "2")%></label>
                                    </div>
                                   <%-- <input type="file" id="Uploaddrawing" runat="server" class="form-control">--%>

                                     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnuploadimg" EventName="Click" />
                                          </Triggers>
                                                <ContentTemplate>
                                   <asp:FileUpload ID="Uploaddrawing" Visible="false" class="form-control" style="background-color: antiquewhite; -webkit-text-stroke-width: thin" runat="server" Width="450px"  />
                                       <asp:HiddenField ID="hdnUploaddrawing" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                       <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="ShowPreview();"/>
                                       <a href="#" id="imgpath" target="_blank"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>

                                               

                                </div>
                            </div>
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img2" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>
                            
                        </div>
                    </div>
           
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>
    <asp:HiddenField ID="hdnsavetype" runat="server" />
    <script type="text/javascript">
                



          function AddNewProjectstatus()
        {
                debugger
                document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatus").style.display = "none";
               if (document.getElementById('<%=ProjectStatus.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatus").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatus.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColor.ClientID%>').value = "";
               }
        }


        function AddNewpriority()
        {
                debugger
                document.getElementById('<%=hdnPriority.ClientID%>').value = "0";
                 document.getElementById("dvnewpriority").style.display = "none";
               if (document.getElementById('<%=Priority.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "1";
                    document.getElementById("dvnewpriority").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewpriority.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').value = "";
               }
        }

      


          function SvaeData(val1A) {
                    debugger
              if (Page_ClientValidate("btnsave"))
              {
                      document.getElementById('<%=hdnsavetype.ClientID%>').value = val1A;
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) 
                        {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }


                       // Project status save
                        
                        var projectstatusid = "0";
                        if (document.getElementById('<%=hdnProjectStatus.ClientID%>').value != "" && document.getElementById('<%=hdnProjectStatus.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addproject.aspx/AddAddprojecttatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatus.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        projectstatusid = response.d;
                                    

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
                            projectstatusid = document.getElementById('<%=ProjectStatus.ClientID%>').value;
                        }

                        // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "Addproject.aspx/AddPrioritytype",
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


                        // save all details

                        
                        var projectdetails = {};
                        projectdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; ;  
                        projectdetails.ProjectID = document.getElementById('<%=ProjectID.ClientID%>').value; 
                        projectdetails.ProjectName = document.getElementById('<%=ProjectName.ClientID%>').value; 
                        projectdetails.Owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        projectdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value; 
                        <%--projectdetails.ProjectStatus = document.getElementById('<%=ProjectStatus.ClientID%>').value;--%> 
                        projectdetails.ProjectStatus = projectstatusid;
                        projectdetails.ProjectAddress1 = document.getElementById('<%=ProjectAddress1.ClientID%>').value; 
                        projectdetails.ProjectAddress2 = document.getElementById('<%=ProjectAddress2.ClientID%>').value; 
                        projectdetails.City = document.getElementById('<%=City.ClientID%>').value; 
                        projectdetails.PostalCode = document.getElementById('<%=PostalCode.ClientID%>').value; 
                        projectdetails.StateName = document.getElementById('<%=StateName.ClientID%>').value; 
                        projectdetails.Countryid = document.getElementById('<%=Countryid.ClientID%>').value; 
                       <%-- projectdetails.ClientName = document.getElementById('<%=ClientName.ClientID%>').value;--%>
                        projectdetails.ClientName =  $("#<%=ddlclientname.ClientID %> option:selected").text();
                       
                        projectdetails.Priority = Prioritytypeid;
                        projectdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        projectdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        projectdetails.AwardDate = document.getElementById('<%=AwardDate.ClientID%>').value; 
                        projectdetails.StartDate = document.getElementById('<%=StartDate.ClientID%>').value; 
                        projectdetails.TargetDate = document.getElementById('<%=TargetDate.ClientID%>').value; 
                        projectdetails.ApprovalDrawing = document.getElementById('<%=ApprovalDrawing.ClientID%>').value; 
                        projectdetails.DrawingApprovalDate = document.getElementById('<%=DrawingApprovalDate.ClientID%>').value; 
                     <%--  projectdetails.Uploaddrawing = document.getElementById('<%=Uploaddrawing.ClientID%>').value; --%>

                        document.getElementById('<%=btnuploadimg.ClientID%>').click();
                     projectdetails.Uploaddrawing = document.getElementById('<%=hdnUploaddrawing.ClientID%>').value;
                  projectdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value; 

                    if (document.getElementById('<%=isdrawing.ClientID%>').checked == true)
                    {

                        projectdetails.isdrawing = 1

                    }

                    if (document.getElementById('<%=isdrawing.ClientID%>').checked == false)
                    {

                        projectdetails.isdrawing = 0

                  }

                       if (document.getElementById('<%=checkprojectWeightage.ClientID%>').checked == true)
                       {

                        projectdetails.checkprojectWeightage = 1

                       }

                       if (document.getElementById('<%=checkprojectWeightage.ClientID%>').checked == false)
                       {

                        projectdetails.checkprojectWeightage = 0

                       }
projectdetails.customerid= document.getElementById('<%=ddlclientname.ClientID%>').value;
        projectdetails.salesorderid = document.getElementById('<%=ddlsalesorder.ClientID%>').value;

                  if (document.getElementById('<%=checkprojecttask.ClientID%>').checked == true)
                       {

                        projectdetails.hastask= 1;

                  }
                  if (document.getElementById('<%=checkprojecttaskWeightage.ClientID%>').checked == true)
                       {

                        projectdetails.checktaskWeightage= 1;

                  }
                  if (document.getElementById('<%=checkprojectsubtask.ClientID%>').checked == true)
                       {

                        projectdetails.hassubtask= 1;

                  }
                  if (document.getElementById('<%=checkprojectsubtaskWeightage.ClientID%>').checked == true)
                       {

                        projectdetails.checksubtaskWeightage= 1;

                       }



                       
                        $.ajax({
                            type: "POST",
                            url: 'Addproject.aspx/SaveProject',
                            data: '{projectdetails: ' + JSON.stringify(projectdetails) + '}',
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
                   // alert(response.d);
                    alert("Data Saved successfully.");
                    document.getElementById('<%=btnload.ClientID%>').click();


                    $("[id*=ProjectID]").val("");
                    $("[id*=ProjectName]").val("");
                    $("[id*=CmpOwner]").val("");
                    $("[id*=team]").val("");
                    $("[id*=ProjectStatus]").val("0");
                    $("[id*=ProjectAddress1]").val("");
                    $("[id*=ProjectAddress2]").val("");
                    $("[id*=City]").val("");
                    $("[id*=PostalCode]").val("");
                    $("[id*=StateName]").val("0");
                    $("[id*=Countryid]").val("0");
                    $("[id*=ClientName]").val("");
                    $("[id*=Priority]").val("0");
                    $("[id*=Budget]").val("");
                    $("[id*=Actual]").val("");
                    $("[id*=AwardDate]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=TargetDate]").val("");
                    $("[id*=ApprovalDrawing]").val("");
                    $("[id*=DrawingApprovalDate]").val("");
                    $("[id*=Uploaddrawing]").val("");
                     document.getElementById('<%=hdnnewclient.ClientID%>').value = "";
                        document.getElementById('<%=hdnclientname.ClientID%>').value = "";
                        document.getElementById('<%=hdnsoid.ClientID%>').value = "";
                     document.getElementById("<%=ProjectStatus.ClientID%>").value = "0";
                     document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=CmpOwner.ClientID%>")).val(0);
                   $(document.getElementById("<%=CmpOwner.ClientID%>")).change();

                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                     document.getElementById('<%=Weightage.ClientID%>').value = "";
                     $(document.getElementById("<%=team.ClientID%>")).val(0);
                    $(document.getElementById("<%=team.ClientID%>")).change();
                    document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = 0;
                    document.getElementById('<%=isdrawing.ClientID%>').checked = 0;
                       document.getElementById('<%=Button1.ClientID%>').click();
                        document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";
                   
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save") 
                    {
                        
                      
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindproject_data();
                    }

                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore") 
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        Bindproject_data();
                    }
                   
                 
                    
                }



          function Cleardata() {
                        
              
                   $("[id*=ProjectID]").val("");
                    $("[id*=ProjectName]").val("");
                    $("[id*=CmpOwner]").val("");
                    $("[id*=team]").val("");
                    $("[id*=ProjectStatus]").val("0");
                    $("[id*=ProjectAddress1]").val("");
                    $("[id*=ProjectAddress2]").val("");
                    $("[id*=City]").val("");
                    $("[id*=PostalCode]").val("");
                    $("[id*=StateName]").val("0");
                    $("[id*=Countryid]").val("0");
                   $("[id*=ClientName]").val("");
                    $("[id*=Priority]").val("0");
                    $("[id*=Budget]").val("");
                    $("[id*=Actual]").val("");
                    $("[id*=AwardDate]").val("");
                    $("[id*=StartDate]").val("");
                    $("[id*=TargetDate]").val("");
                    $("[id*=ApprovalDrawing]").val("");
                    $("[id*=DrawingApprovalDate]").val("");
                   $("[id*=Uploaddrawing]").val("");

           

document.getElementById('<%=hdnid.ClientID%>').value = "";
                    document.getElementById('<%=CmpOwner.ClientID%>').value = "";
                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                  document.getElementById('<%=hdnCountryid.ClientID%>').value = "";
              document.getElementById('<%=hdnstatename.ClientID%>').value = "";
              document.getElementById('<%=ProjectStatus.ClientID%>').value = "0";
              document.getElementById('<%=hdnProjectStatus.ClientID%>').value = "0";
               document.getElementById('<%=Priority.ClientID%>').value = "0";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "0";

              document.getElementById('<%=AwardDate.ClientID%>').value = "";
              document.getElementById('<%=StartDate.ClientID%>').value = "";
              document.getElementById('<%=TargetDate.ClientID%>').value = "";
              document.getElementById('<%=TargetDate.ClientID%>').value = "";

                    document.getElementById('<%=hdnawarddate.ClientID%>').value = "";
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = "";
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = "";
                    document.getElementById('<%=hdndrawingaprvldate.ClientID%>').value = "";
                 document.getElementById('<%=hdnteam.ClientID%>').value = "";
              document.getElementById('<%=Weightage.ClientID%>').value = "";

               document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = 0;
                 document.getElementById('<%=isdrawing.ClientID%>').checked = 0;

                   $(document.getElementById("<%=CmpOwner.ClientID%>")).val(0);
                  $(document.getElementById("<%=CmpOwner.ClientID%>")).change();

                  $(document.getElementById("<%=team.ClientID%>")).val(0);
              $(document.getElementById("<%=team.ClientID%>")).change();

                        document.getElementById("imgpath").href = "#";
                        document.getElementById("empimg").src = "/assets/images/user.jpg";

                   document.getElementById('<%=Button1.ClientID%>').click();
                
         
                        
                    }

        //$(document).ready(function IsPostBack() {
        //    Binddetails();
        //});


       

          function Binddetails(RecordId) {

          
                debugger;
                 
                $.ajax({
                    url: 'Addproject.aspx/Bindprojectdetail',
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
                var projectdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (projectdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = projectdetails.find("id").text();
                    
                    $("[id*=ProjectID]").val(projectdetails.find("ProjectID").text());
                    $("[id*=ProjectName]").val(projectdetails.find("ProjectName").text());
                    $("[id*=CmpOwner]").val(projectdetails.find("CmpOwner").text());
                    $("[id*=team]").val(projectdetails.find("team").text()); 
                    //$("[id*=ProjectStatus]").val(projectdetails.find("ProjectStatus").text());
                    $("[id*=ProjectAddress1]").val(projectdetails.find("ProjectAddress1").text());
                    $("[id*=ProjectAddress2]").val(projectdetails.find("ProjectAddress2").text()); 
                    $("[id*=City]").val(projectdetails.find("City").text()); 
                    $("[id*=PostalCode]").val(projectdetails.find("PostalCode").text()); 
                    $("[id*=StateName]").val(projectdetails.find("StateName").text()); 
                    
                    $("[id*=ClientName]").val(projectdetails.find("ClientName").text()); 
                    //$("[id*=Priority]").val(projectdetails.find("Priority").text()); 
                    $("[id*=Budget]").val(projectdetails.find("Budget").text()); 
                    $("[id*=Actual]").val(projectdetails.find("Actual").text()); 
                    $("[id*=AwardDate]").val(projectdetails.find("AwardDate").text()); 
                    $("[id*=StartDate]").val(projectdetails.find("StartDate").text()); 
                    $("[id*=TargetDate]").val(projectdetails.find("TargetDate").text()); 
                    $("[id*=ApprovalDrawing]").val(projectdetails.find("ApprovalDrawing").text()); 
                    $("[id*=DrawingApprovalDate]").val(projectdetails.find("DrawingApprovalDate").text()); 

                    $("[id*=Countryid]").val(projectdetails.find("Countryid").text());
                    try {
                        if (projectdetails.find("hastask").text() == 1) {
                            document.getElementById('<%=checkprojecttask.ClientID%>').checked = true;
                            showtaskdetails('ContentPlaceHolder1_checkprojecttask');
                        }
                        if (projectdetails.find("checktaskWeightage").text() == 1) {
                            document.getElementById('<%=checkprojecttaskWeightage.ClientID%>').checked = true;
                            
                        }
                    }
                    catch{ }
                    try {
                        if (projectdetails.find("hassubtask").text() == 1) {
                            document.getElementById('<%=checkprojectsubtask.ClientID%>').checked = true;
                            showsubtaskdetails('ContentPlaceHolder1_checkprojectsubtask');
                        }
                        if (projectdetails.find("checksubtaskWeightage").text() == 1) {
                            document.getElementById('<%=checkprojectsubtaskWeightage.ClientID%>').checked = true;
                            
                        }
                    }
                    catch{}
                    //if (projectdetails.find("Uploaddrawing").text() != "")
                    //{
                        
                    //    document.getElementById("empimg").src = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    //    document.getElementById("imgpath").href = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    //}
                    //else
                    //{
                    //    document.getElementById("imgpath").href = "#";
                    //    document.getElementById("empimg").src = "/assets/images/user.jpg";
                    //}


                    if (projectdetails.find("Uploaddrawing").text() != "" && projectdetails.find("Uploaddrawing").text() != "0")
                    {
                        
                        document.getElementById("empimg").src = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                        document.getElementById("imgpath").href = "/assets/images/" + projectdetails.find("Uploaddrawing").text();
                    }
                    

                    
                    document.getElementById('<%=hdnowner.ClientID%>').value = projectdetails.find("Owner").text();
                    document.getElementById('<%=hdnCountryid.ClientID%>').value = projectdetails.find("Countryid").text();
                    document.getElementById('<%=hdnstatename.ClientID%>').value = projectdetails.find("StateName").text();

                    document.getElementById('<%=hdnclientname.ClientID%>').value = projectdetails.find("customerid").text();
                    document.getElementById('<%=hdnsoid.ClientID%>').value = projectdetails.find("salesorderid").text();

                     document.getElementById('<%=ProjectStatus.ClientID%>').value = projectdetails.find("ProjectStatus").text();
                    document.getElementById('<%=hdnProjectStatus.ClientID%>').value = projectdetails.find("ProjectStatus").text();
                    
                    document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";
                
                     document.getElementById('<%=Priority.ClientID%>').value = projectdetails.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = projectdetails.find("Priority").text();

                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";
                  
                    document.getElementById('<%=hdnawarddate.ClientID%>').value = projectdetails.find("AwardDate").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = projectdetails.find("StartDate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = projectdetails.find("TargetDate").text();
                    document.getElementById('<%=hdndrawingaprvldate.ClientID%>').value = projectdetails.find("DrawingApprovalDate").text();
                    document.getElementById('<%=hdnteam.ClientID%>').value = projectdetails.find("team").text();
                    document.getElementById('<%=Weightage.ClientID%>').value = projectdetails.find("Weightage").text();


                  if (projectdetails.find("isdrawing").text() == 1)
                  {
                      document.getElementById('<%=isdrawing.ClientID%>').checked = true;
                      document.getElementById("divaprovaldraing").style.display = "block";
                      document.getElementById("divaprovalDate").style.display = "block";
                      document.getElementById("divuploaddrawing").style.display = "block";

                  }

                  if (projectdetails.find("isdrawing").text() == 0)
                  {
                      document.getElementById('<%=isdrawing.ClientID%>').checked = false;
                      document.getElementById("divaprovaldraing").style.display = "none";
                      document.getElementById("divaprovalDate").style.display = "none";
                      document.getElementById("divuploaddrawing").style.display = "none";

                    }


                   if (projectdetails.find("checkprojectWeightage").text() == 1)
                   {
                      document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = true;

                   }

                   if (projectdetails.find("checkprojectWeightage").text() == 0)
                   {
                      document.getElementById('<%=checkprojectWeightage.ClientID%>').checked = false;

                    }


                   
                   

               if (projectdetails.find("Owner").text() != "")
             {
             var idsemp = projectdetails.find("Owner").text().split(',');
            $(document.getElementById("<%=CmpOwner.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=CmpOwner.ClientID%>")).change();
            }

              if (projectdetails.find("team").text() != "")
             {
             var idsemp = projectdetails.find("team").text().split(',');
            $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=team.ClientID%>")).change();
            }


                   
                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   
                     //document.getElementById("btnsaveandmore").style.display = "none";
                }
            }
            catch
                {
                }
binddate();
            }


        
        


            </script>
     <script>
    function closesalespoup()
    {
debugger
if(document.getElementById("<%=hdnnewclient.ClientID %>").value!=""&&document.getElementById("<%=hdnnewclient.ClientID %>").value!="0")
{
alert(document.getElementById("<%=hdnnewclient.ClientID %>").value);
document.getElementById("<%=btnbindclint.ClientID %>").click(); 
document.getElementById("btnclsoesales").click();
}
else
{ document.getElementById("btnclsoesales").click();}

    }
    </script>

      <%-- show column hide show end--%>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.3/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/ui/1.13.3/jquery-ui.js"></script>
  <script>
  
   <%--function bindDatePickers() {
        $("#<%=AwardDate.ClientID %>").datepicker({
            todayBtn: "linked",
            language: "it",
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=StartDate.ClientID %>").datepicker({
            todayBtn: "linked",
            language: "it",
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=TargetDate.ClientID %>").datepicker({
            todayBtn: "linked",
            language: "it",
            autoclose: true,
            todayHighlight: true,
            dateFormat: 'dd/mm/yy'
        });
    }

    // Bind on first page load
    $(function () {
        bindDatePickers();
    });--%>

    // Bind again after every partial postback (UpdatePanel refresh)
    Sys.Application.add_load(function () {
        bindDatePickers();
    });

    
function binddate()
            {
debugger
try {
                   
                     if (document.getElementById("<%=hdnawarddate.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnawarddate.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=AwardDate.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }

try {
                   
                     if (document.getElementById("<%=hdnstartdate.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdnstartdate.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=StartDate.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }

try {
                   
                     if (document.getElementById("<%=hdntargetdate.ClientID%>").value != "") {
                    const today = new Date(document.getElementById("<%=hdntargetdate.ClientID%>").value);
                    const yyyy = today.getFullYear();
                    let mm = today.getMonth() + 1;
                    let dd = today.getDate();
                    if (dd < 10) dd = '0' + dd;
                    if (mm < 10) mm = '0' + mm;
                    const formattedToday = dd + '/' + MM + '/' + yyyy;
                    document.getElementById('<%=TargetDate.ClientID%>').value = formattedToday;
                }
                }
                catch
                {

                }
}
  </script>
    <script>
        function showtaskdetails(ctrlid) {
        debugger
        if (document.getElementById(ctrlid).checked) {
            document.getElementById('dvprojectsubtask').style.display = '';
            document.getElementById('dvprojecttaskWeightage').style.display = '';
            
        }
        else {
            document.getElementById('dvprojectsubtask').style.display = 'none';
            document.getElementById('dvprojecttaskWeightage').style.display = 'none';
        }
    }
        function showsubtaskdetails(ctrlid) {
        debugger
        if (document.getElementById(ctrlid).checked) {
            document.getElementById('dvprojectsubtaskWeightage').style.display = '';
            
        }
        else {
            document.getElementById('dvprojectsubtaskWeightage').style.display = 'none';
            
        }
    }
</script>
    <script type="text/javascript">
    Sys.Application.add_load(function () {
        $('.chzn-select').each(function () {
            var $this = $(this);
            if ($this.data('chosen')) {
                $this.chosen('destroy');
            }
            $this.chosen(); // re-initialize Chosen
        });
    });
</script>
     <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
<script src="/pms/assets/js/select2.min.js"></script> 
       <%--Multi dropdown select start --%> 

    <script>
    debugger
    var ddLOwner = $(document.getElementById("<%=CmpOwner.ClientID%>"));
     $(ddLOwner).select2({
        data: [
    <%=MultiOwner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        width: "100%"
    });

   
     $(document.getElementById("<%=CmpOwner.ClientID%>")).change(function () {
        debugger
        var ids = $(document.getElementById("<%=CmpOwner.ClientID%>")).val();
        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
        document.getElementById("<%=CmpOwner.ClientID%>").click();

    });
   
  
    </script>
    

    <script>
    debugger
    var ddLTeam = $(document.getElementById("<%=team.ClientID%>"));
    $(ddLTeam).select2({
        data: [
    <%=Multiteam%>
        ],
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
   
  
    </script>



    

    

    <%--Multi dropdown select start end--%> 


   <%-- image upload start--%>


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
                url: "FileUploadHandler.ashx?id="+id+"", 
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
                    return;
                }
            }  
        }  
</script>

    <%-- image upload end--%>

</asp:Content>

