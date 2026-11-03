<%@ Page Language="C#" AutoEventWireup="true" CodeFile="holidaysetup.aspx.cs" Inherits="holidaysetup" %>
<%--<%@ Register Src="~/hrmssetting/DateSelector/DateSelector.ascx" TagName="date"  TagPrefix="date" %>--%>
<%@ Register src="/settings/ucontrol/leftmenhr.ascx" tagname="leftmenupreferences" tagprefix="uc1" %>
 <%@ Register src="~/employee/usercontrol/datepicker.ascx" tagname="date" tagprefix="date" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-touch-fullscreen" content="yes">
    <meta name="author" content="Absolutewebtech">
       <link href="/pms/assets/js/jquery.dataTables.min.css" rel="stylesheet" type="text/css"> 
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400italic,600' rel='stylesheet' type='text/css'>
    <link type="text/css" href="/hrmssetting/assets/plugins/iCheck/skins/minimal/blue.css" rel="stylesheet">
    <link type="text/css" href="/hrmssetting/assets/fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <link type="text/css" href="/hrmssetting/assets/fonts/themify-icons/themify-icons.css" rel="stylesheet"> 
    <link type="text/css" href="/hrmssetting/assets/css/styles.css" rel="stylesheet">
    <script src="/hrmssetting/assets/js/jquery.min.js" type="text/javascript"></script>
  
    
       <style>
        table tr td input, table tr td select, table tr td textarea
        {
           width: auto !important;
        }
        .input100{
             width: 100% !important;
        }
        .modal {
    z-index: 9999;
}
        table tr td label {
    display: inline !important;
    margin-left:5px !important;
    
}
.dataTables_wrapper .dataTables_length select {padding:0;}
.search-box {float:right;}
.dataTables_wrapper {margin-top:25px;}
    </style> 
     <link type="text/css" href="/settings/assets/css/approval.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">

 <header id="topnav" style="display:none;" class="navbar navbar-default navbar-fixed-top" role="banner">
            <div class="title-bar">
                <h5><%=fns.getltext("HR", "2")%></h5>  
                
                </div>
            </header>
            
       
           
                   
                    
                   
                             <div class="col-sm-12" style="border-left:1px solid #ccc;">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div id="message" runat="server" style="margin-top:10px; margin-left:40px; font-size:medium; font-weight:bold; color:Green;"  visible="false"></div>

 <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div class="divWaiting" onclick="alert('Please wait data is loading...');"> 
                                <img src='/Images/loading1.gif'  />
                               
  </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
<ContentTemplate>



<div id="pnlcloase" runat="server" visible="false"  style="float:right;"  >
<a href="holidaysetup.aspx">
<img src="/images/cross.gif" /></a>
</div>

<asp:Panel ID="pnlholidaymanage" runat="server" Visible="true">
<div class="start-headingtop" style="float:left; text-align:left;">

       <h2><%=fns.getltext("Manage Holidays", "2")%><asp:Label  ID="lblholidaylistname" runat="server"></asp:Label></h2>
       
        
       <%-- <a role="button" data-toggle="modal" data-target="#myModalform1" class="btn btn-primary btn-right" href="javascript:void(0);">
       + <%=fns.getltext("Create Holiday", "2")%>

       </a>--%>
       
</div>
    <div class="form-horizontal row-border">
                                       <table  class="table table-bordered">
     <tr><td><%=fns.getltext("Holiday Name", "2")%></td><td><asp:TextBox ID="txtnameholiday" runat="server" CssClass="input100"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter holiday name"  
         Display="Dynamic" SetFocusOnError="True" ValidationGroup="pnl2" ControlToValidate="txtnameholiday"></asp:RequiredFieldValidator>
       
        </td></tr>
       <tr><td><%=fns.getltext("Holiday Detail", "2")%></td><td><asp:TextBox ID="txtholidaydesc" CssClass="input100" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox> </td></tr>

        <tr><td><%=fns.getltext("Date", "2")%></td><td><date:date ID="eventdate" runat="server" requeredEnabled="true" requeredDisplay="Dynamic" ValidationGroup="pnl2" />  </td></tr>
          <tr><td colspan="2"><asp:CheckBox ID="chkisnationalholiday" runat="server" /> <%=fns.getltext("This is a National holiday", "2")%>
      
                                           <tr style="display:none;"><td colspan="2"><asp:CheckBox ID="chkisfloating" runat="server" /> <%=fns.getltext("This is a floating holiday", "2")%>
          </td></tr>
        <tr><td colspan="2">
        <center>
        <asp:Button ID="btnaddholiday" runat="server" Text="Save" OnClick="btnaddholiday_click" CssClass="btn btn-success" ValidationGroup="pnl2" /> &nbsp;<asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="btn btn-inverse"  OnClick="btncancelholiday_click"  /> 
        </center>
         </td></tr>


     </table>
                                    
                                
             <asp:HiddenField ID="hdndefaulttypeid" runat="server"></asp:HiddenField>                            
                              </div>
       <%--<p><%=fns.getltext("You can specify list of holidays here. These can be assigned to specific holiday list", "2")%></p>  --%>

       <asp:GridView ID="grdholidaylist" runat="server" ShowHeaderWhenEmpty="True"
             AutoGenerateColumns="False" CssClass="table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable holiday_tbl" 
             DataKeyNames="id"
           
           
             onrowdatabound="grduser_RowDataBound" OnPreRender="grdholidaylist_PreRender" Width="100%"> 
     
          
 
            <Columns>
 
            <asp:TemplateField >
            <HeaderTemplate><%=fns.getltext("Holiday Name", "2")%></HeaderTemplate>
            <ItemTemplate>
               
               
                 
     <%#Eval("title")%> <%#showfloating(Eval("isfloating").ToString())%><br />
      <%#Eval("leavedesc")%>
                
            </ItemTemplate>
            </asp:TemplateField>
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Date", "2")%></HeaderTemplate>
            <ItemTemplate>
           <%#Eval("eventdate","{0:dd-MMM-yyyy}").ToString()%>
            </ItemTemplate>
            </asp:TemplateField>
           
           
            
          
            
            
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Operation", "2")%></HeaderTemplate>
            <ItemTemplate>         
                   &nbsp;&nbsp;
                    
            <asp:LinkButton ID="lnkedit" runat="server" onclick="lnkeditholiday_Click"><span class="btn btn-success btn-sm"><i class="fa fa-edit"></i></span></asp:LinkButton>&nbsp;
           <asp:LinkButton ID="lnkdelete" runat="server" onclick="lnkdeleteholiday_Click" OnClientClick="return confirm('Are You Sure To Delete This Entry?');" ><span class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></span></asp:LinkButton>&nbsp; &nbsp; 
            </ItemTemplate>
            
                
            </asp:TemplateField> 
                      
            </Columns>
            <EmptyDataTemplate>
                      <%=fns.getltext("No record available ", "2")%>                   
            </EmptyDataTemplate>       
        </asp:GridView>                          

</asp:Panel>

     <div class="modal fade in inner-modal"  runat="server" id="myModalform1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" style="padding:0px !important;display:none;">
    <div class="modal-dialog"> 
        <div class="modal-content">
        	<%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true"> 
            	<span><i class="fa fa-remove"></i></span>
            </button>--%>
           
             <div class="form-submit">
               
                                 
                                     <div class="start-heading">
                                            <h2><%=fns.getltext("Create Holiday", "2")%></h2>
                                        </div>
                                        
            </div>
        </div>
    </div>
</div>
                                   
       


<asp:Panel ID="pnlholidaylist" runat="server" Visible="false">

   <div class="start-headingtop" style="float:left; text-align:left;">
       <h2><%=fns.getltext("List of Permitted Holidays", "2")%></h2>
       <a role="button" data-toggle="modal" data-target="#myModalform" class="btn btn-primary btn-right" href="javascript:void(0);">
       + <%=fns.getltext("Create Holiday", "2")%>

       </a>


       
   </div>
       <%--<p><%=fns.getltext("Here you can manage all the holiday list for your employees", "2")%></p>--%>
       
       
<asp:GridView ID="grduser" runat="server" ShowHeaderWhenEmpty="True"
             AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" 
             DataKeyNames="id" Width="100%"
           
           
             onrowdatabound="grduser_RowDataBound"> 
     
          
 
            <Columns>
 
            <asp:TemplateField >
            <HeaderTemplate><%=fns.getltext("Holiday Name", "2")%></HeaderTemplate>
            <ItemTemplate>
     <%#Eval("listname")%>
                
            </ItemTemplate>
            </asp:TemplateField>
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Applied to Whom", "2")%></HeaderTemplate>
            <ItemTemplate>
           <%#showapplies(Eval("id").ToString()) %>
            </ItemTemplate>
            </asp:TemplateField>
           
           
             <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("No of Holidays", "2")%></HeaderTemplate>
            <ItemTemplate>
         <%#shownoofholiday(Eval("id").ToString()) %>
            </ItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Click to make Default", "2")%></HeaderTemplate>
            <ItemTemplate>
            <asp:HiddenField ID="hdndefault" runat="server" Value=<%#Eval("isdefault")%> />

             <asp:LinkButton ID="linksetdefault" runat="server" onclick="linksetdefault_Click">   <image src="/Images/checkmarkdefault.png" title="This is default list" ></asp:LinkButton>
              <asp:LinkButton ID="linksetdefault1" runat="server" onclick="linksetdefault1_Click">   <image src="/Images/checkmarkddefault1.png"  title="Set as default"></asp:LinkButton>
          
        
            </ItemTemplate>
            </asp:TemplateField>
            
            
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Operation", "2")%></HeaderTemplate>
            <ItemTemplate>         
                   &nbsp;&nbsp;
                   <asp:LinkButton ID="lnkmanage" runat="server" onclick="lnkmanage_Click" class="btn btn-primary btn-sm"><%=fns.getltext("Action", "2")%></asp:LinkButton>&nbsp;    
            <asp:LinkButton ID="lnkedit" runat="server" onclick="lnkedit_Click"><span class="btn btn-success btn-sm"><i class="fa fa-edit"></i></span></asp:LinkButton>&nbsp; &nbsp;
         
            </ItemTemplate>
            
                
            </asp:TemplateField> 
                      
            </Columns>
            <EmptyDataTemplate>
                      <%=fns.getltext("No record available", "2")%>                   
            </EmptyDataTemplate>       
        </asp:GridView>

                                        

 
  
  </asp:Panel>

  <asp:HiddenField ID="hdnmodel1" runat="server" />
     <div class="modal fade in inner-modal" runat="server" id="myModalform" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" style="padding:0px !important; display:none;">
    <div class="modal-dialog"> 
        <div class="modal-content">
     
            
             <div class="form-submit">
               
                                 
                                     <div class="start-heading">
                                            <h2><%=fns.getltext("Create Holiday Policy", "2")%></h2>
                                        </div>
                                        <div class="form-horizontal row-border">
                                       <table  class="table table-bordered">
     <tr><td><%=fns.getltext("Holiday Policy Name", "2")%></td></tr>
       <tr><td><asp:TextBox ID="txtlistname" runat="server" CssClass="input100"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="Please Eneter Holiday Policy  Name"  Display="Dynamic" SetFocusOnError="True" ValidationGroup="pnl1" ControlToValidate="txtlistname"></asp:RequiredFieldValidator>
       
        </td></tr>
       <tr><td><%=fns.getltext("Holiday Policy  Detail", "2")%></td></tr>
       <tr><td><asp:TextBox ID="txtlistdesc" runat="server" CssClass="input100" TextMode="MultiLine" Width="100%"></asp:TextBox> </td></tr>
        <tr><td><asp:Button ID="btnadd" runat="server" Text="Save" OnClick="btnadd_click" CssClass="btn btn-success" ValidationGroup="pnl1" /> &nbsp;<asp:Button ID="Button1" runat="server" CssClass="btn btn-inverse" Text="Cancel" OnClick="btncancel_click"  />  </td></tr>
     </table>
                                    
                                   
                              </div>
            </div>
        </div>
    </div>
</div>



   
    </ContentTemplate>
    </asp:UpdatePanel>


 
<asp:Panel ID="Panel1" runat="server" Visible="false">
<div class="start-headingtop" style="float:left; text-align:left;">

       <h2><%=fns.getltext("Holiday", "2")%></h2>
       
        
        <a role="button" data-toggle="modal" data-target="#myModalform2" class="btn btn-primary btn-right" href="javascript:void(0);">
       + <%=fns.getltext("Create Restricted Holiday", "2")%>

       </a>
       
</div>
       <p style="display:none"><%=fns.getltext("You can specify list of Restricted holidays here.", "2")%></p>  

       <asp:GridView ID="GridView1" runat="server" ShowHeaderWhenEmpty="True"
             AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" 
             DataKeyNames="id" 
             onrowdatabound="grduser_RowDataBound"> 
     
          
 
            <Columns>
 
            <asp:TemplateField >
            <HeaderTemplate><%=fns.getltext("Holiday Name", "2")%></HeaderTemplate>
            <ItemTemplate>
               
               
                 
     <%#Eval("title")%>   
      <%#Eval("leavedesc")%>
                
            </ItemTemplate>
            </asp:TemplateField>
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Date", "2")%></HeaderTemplate>
            <ItemTemplate>
           <%#Eval("eventdate","{0:dd-MMM-yyyy}").ToString()%>
            </ItemTemplate>
            </asp:TemplateField>
           
           
            
          
            
            
 
            <asp:TemplateField>
            <HeaderTemplate><%=fns.getltext("Operation", "2")%></HeaderTemplate>
            <ItemTemplate>         
                   &nbsp;&nbsp;
                    
            <asp:LinkButton ID="lnkedit" runat="server" onclick="lnkeditholiday1_Click"><span class="btn btn-success btn-sm"><i class="fa fa-edit"></i></span></asp:LinkButton>&nbsp;
           <asp:LinkButton ID="lnkdelete" runat="server" onclick="lnkdeleteholiday1_Click" OnClientClick="return confirm('Are You Sure To Delete This Entry?');" ><span class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></span></asp:LinkButton>&nbsp; &nbsp; 
            </ItemTemplate>
            
                
            </asp:TemplateField> 
                      
            </Columns>
            <EmptyDataTemplate>
                      <%=fns.getltext("No record available", "2")%>                   
            </EmptyDataTemplate>       
        </asp:GridView>                          

     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div class="divWaiting" onclick="alert('Please wait data is loading..');"> 
                                <img src='/Images/loading1.gif' />
                               
  </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
<ContentTemplate>
                              

                                     <table  class="table table-striped table-bordered table-hover" style="display:none">
                                          <tr> <td colspan="2"><b><%=fns.getltext("Limit of Restricted holiday days per year", "2")%> </b>
                                              <asp:TextBox ID="txtnoofrholiday" runat="server" type="number" onkeypress="return isnumeric(event)"
                                                  min="0" max="500" step="1" style="width:45px !important; text-align:center;"></asp:TextBox> </td>
                                              

                                              </tr>
                                          <tr> <td  colspan="2"><b><%=fns.getltext("Leave request must be made no sooner than", "2")%>
                                              <asp:TextBox ID="txtsoonerday" runat="server" type="number" onkeypress="return isnumeric(event)" 
                                                  min="0" max="500" step="1" style="width:45px !important; text-align:center;"></asp:TextBox>  
                                               <%=fns.getltext("calendar days prior to the leave date", "2")%> </b>&nbsp;&nbsp;</td> 
                                              </tr>
                                           <tr> 
    <td colspan="2"><b> <%=fns.getltext("Allow an employee to apply leaves for back dates?", "2")%></b></td>

   </tr>
  <tr><td colspan="2"><asp:RadioButton ID="chkapplybackdate1" runat="server" GroupName="applybackdate" Checked="true" AutoPostBack="true" OnCheckedChanged="chkapplybackdate_change"  /> <%=fns.getltext("Yes. Employee can apply leave for past days, but no beyond than", "2")%>
  <asp:TextBox ID="txtapplybackdate" runat="server" onkeypress="return isnumeric(event)" Enabled="true" style="width:45px !important; text-align:center;" TextMode="Number" min="0" max="500" step="1"></asp:TextBox>&nbsp; <%=fns.getltext("calendar days back. This option will ignore prior notice rule", "2")%>
  <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="required."  Display="Dynamic" SetFocusOnError="True" 
     ValidationGroup="pnl3" ControlToValidate="txtapplybackdate" Enabled="true"></asp:RequiredFieldValidator>
  
  </td></tr>
    <tr><td colspan="2"><asp:RadioButton ID="chkapplybackdate2" runat="server" GroupName="applybackdate"  AutoPostBack="true" OnCheckedChanged="chkapplybackdate_change"  /> <%=fns.getltext("No. Employees cannot apply for past days.", "2")%>
 </td></tr>
    <tr>
    <td colspan="2"><b><%=fns.getltext("Does Restricted Holiday require an approval?", "2")%></b>&nbsp;&nbsp;</td> </tr>
 
    <tr><td style="width:100px;"><asp:RadioButton ID="rdlleaveapproval1" runat="server" GroupName="rdlleaveapproval" CssClass="radibuttonlistclass"  AutoPostBack="true" OnCheckedChanged="rdlleaveapproval_change" />&nbsp;<%=fns.getltext("Yes", "2")%></td>
   <td><asp:RadioButton ID="rdlleaveapproval2" runat="server" GroupName="rdlleaveapproval" CssClass="radibuttonlistclass" Checked="true" AutoPostBack="true" OnCheckedChanged="rdlleaveapproval_change"  />&nbsp;<%=fns.getltext("No", "2")%>
  </td></tr>
  <tr><td colspan="2">
       <asp:Panel ID="pnlleaveapprovelabel" runat="server"  Style=" display:none;" >
                         <table  class="table table-striped table-bordered table-hover">
    <tr style="display:none;">
    <td colspan="2"><b><%=fns.getltext("Does require an Email Notification For Approval?", "2")%></b>&nbsp;&nbsp;</td> </tr>
 
    <tr  style="display:none;"><td style="width:100px;"><asp:RadioButton ID="rdlemailnotification1" runat="server" GroupName="rdlemailnotificationapproval" CssClass="radibuttonlistclass"   />&nbsp;<%=fns.getltext("Yes", "2")%></td>
   <td><asp:RadioButton ID="rdlemailnotification2" runat="server" GroupName="rdlemailnotificationapproval" CssClass="radibuttonlistclass" Checked="true"  />&nbsp;<%=fns.getltext("No", "2")%>
  </td></tr>
                             </table>



            <div onclick="closealldiv()" style="width:100%;">
             <div class="col-lg-12 col-md-12 col-sm-12 nav-list clear-side-padding">
                          	<h4 style="padding:10px 15px; margin:0; font-size:20px;"><%=fns.getltext("Approval Chain", "2")%></h4>
                          	<p style="padding:0px 15px 0px 15px;"><%=fns.getltext("Here you can manage approvers for Restricted Holiday.", "2")%></p>
                           </div>
                          
                          
                          	<div class="col-sm-12">
                            	<div class="">

                                    <asp:LinkButton ID="addnewrow" CssClass="btn btn-primary" runat="server" Text="+ Add Approver Level" OnClick="addnewrow_click" CausesValidation="false"></asp:LinkButton>
                                    <asp:HiddenField ID="hdngridviewrow" runat="server" Value="1" />
                                   
                                    <span class="groups">
                                    	<span><i class='fa fa-users'></i> <%=fns.getltext("Role Based", "2")%></span>  
                                        <span><i class="fa fa-user"></i> <%=fns.getltext("Employees", "2")%></span>
                                    </span>
                                </div>
                            </div>
                </div>
                            <div class="user-roles">
                                <div class="col-sm-12 showcase-item">


                                       <asp:GridView ID="gird1" runat="server" OnRowDataBound="gird1_rowdatabound"  AutoGenerateColumns="false"  ShowHeader="false" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                           <div style="width:100%;" onclick="closealldiv()"><span class="fa fa-square spot"></span><b><asp:Label ID="lblapproballavel" runat="server"></asp:Label></b></div>

                                   <div class="form-group">
                            <asp:HiddenField ID="hdnempids" runat="server" />
                            
                        	<div class="" id="empdata"  style="float:left;" runat="server" onclick="closealldiv()">                             
                            </div>   
                                <div class="input-group input-icon right">
                                    <span class="input-group-addon" onclick="closealldiv()">
                                        <i class="fa fa-user"></i>
                                    </span>
                                    <i class="fa fa-exclamation-circle tooltips" ></i>
                                    <asp:TextBox ID="txtapproval" runat="server" CssClass="form-control"  autocomplete="off"></asp:TextBox> 
                                    <div id="searchdata" runat="server">
                                </div>
                                </div>
                        </div>


                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
            <div style="display:none;">
                <asp:Button ID="btnreset" runat="server" Text="reset" OnClick="btnreset_click" />
            </div>

       
        </div>
        <div onclick="closealldiv()" style="height:100px; width:100%;">
            &nbsp;
            </div>

         <script type="text/javascript">


                                    

                                        var count = 1;

             function searchempdetails(txtval, hdnempids, searchdata, empdata, txtnotify) {


                  var hdngridviewrowval = document.getElementById("<%=hdngridviewrow.ClientID %>").value;
            
                 for (var x = 0; x < hdngridviewrowval; x++)
                 {
                    
                     var divid = "gird1_searchdata_" + x;
                     if (searchdata != divid) {
                         document.getElementById(divid).innerHTML = "";
                     }
                    
                 }


                                            var hdempidval = document.getElementById(hdnempids).value;
                                          //alert("ajax/showempdataapproval.aspx?searchtxt=" + txtval + "&count=" + count + "&hdempidval=" + hdempidval+"&hdnempids="+hdnempids+"&searchdata="+searchdata+"&empdata="+empdata+"&txtnotify="+txtnotify);
                                            $.ajax({
                                                url: "/employee/ajax/showempdataapproval.aspx?searchtxt=" + txtval + "&count=" + count + "&hdempidval=" + hdempidval+"&hdnempids="+hdnempids+"&searchdata="+searchdata+"&empdata="+empdata+"&txtnotify="+txtnotify, success: function (result) {
                                                   
                                                    document.getElementById(searchdata).innerHTML = result;
                                                  

                                                }
                                            })

                                            count++;
             }


          
             function searchempend(txtval,hdnempids,searchdata,empdata,txtnotify){
                 $("#" + txtnotify).val("");
                 document.getElementById(searchdata).innerHTML = "";
                

             }
             function selectemp(id, name, hdnempids, empdata, searchdata, txtnotify,type) {
                                            
                                            var hdempidval = document.getElementById(hdnempids).value;
                                            if (hdempidval != '') {
                                                hdempidval = hdempidval+"," + id + "~"+type;
                                            }
                                            else {
                                                 hdempidval = hdempidval+ id + "~"+type;;
                                            }
                 $("#" + hdnempids).val(hdempidval);


                //  alert("ajax/showempdataselectedapproval.aspx?searchtxt=" + hdempidval+"&count="+count+"&hdnempids="+hdnempids+"&searchdata="+searchdata+"&empdata="+empdata+"&txtnotify="+txtnotify);
                                            
                                              $.ajax({
                                                url: "/employee/ajax/showempdataselectedapproval.aspx?searchtxt=" + hdempidval+"&count="+count+"&hdnempids="+hdnempids+"&searchdata="+searchdata+"&empdata="+empdata+"&txtnotify="+txtnotify, success: function (result) {
                                                    document.getElementById(empdata).innerHTML = result;
                                                }
                                            })
                                            document.getElementById(searchdata).innerHTML = "";
                                            $("#"+txtnotify).val("");
                                            count++;
                                        }
                                          function deleteemp(id,hdnempids,empdata,searchdata,txtnotify) {
                                            var hdempidval = document.getElementById(hdnempids).value.replace(id,"");
                                              $("#"+hdnempids).val(hdempidval);
                                             // alert("ajax/showempdataselected.aspx?searchtxt=" + hdempidval + "&count=" + count);
                                              $.ajax({
                                                url: "/employee/ajax/showempdataselectedapproval.aspx?searchtxt=" + hdempidval+"&count="+count+"&hdnempids="+hdnempids+"&searchdata="+searchdata+"&empdata="+empdata+"&txtnotify="+txtnotify, success: function (result) {
                                                    document.getElementById(empdata).innerHTML = result;

                                                    if (result == '') {

                                                        $("#" + hdnempids).val('');
                                                        $("#<%=btnreset.ClientID %>").click();
                                                    }

                                                    // alert(result);

                                                }
                                            })
                                            document.getElementById(searchdata).innerHTML = "";
                                            $("#"+txtnotify).val("");
                                            count++;
                                        }

             function closealldiv() {
                 var hdngridviewrowval = document.getElementById("<%=hdngridviewrow.ClientID %>").value;
            
                 for (var x = 0; x < hdngridviewrowval; x++)
                 {
                     var divid = "gird1_searchdata_"+x;
                     document.getElementById(divid).innerHTML = "";
                     var apptxtid = "gird1_txtapproval_"+x;
                     $("#"+apptxtid).val("");
                 }
             }

                                    </script> 




                                    
                                 
                                </div>
                           
						  

       </asp:Panel>
   
  </td>
      </tr>
   </table>

    </ContentTemplate>
          </asp:UpdatePanel>
      <table  class="table table-striped table-bordered table-hover" style="display:none;">
                                    <tr>
                                        <td>
                                        	<hr>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <center>
                                      <asp:Button ID="btnsave" runat="server" Text="Save" Width="150px" OnClick="btnsave_click" class="btn btn-success" />
                                      </center>
                                           
                                        </td>
                                    </tr>
                                </table>

</asp:Panel>


          <div class="modal fade in inner-modal"  runat="server" id="myModalform2" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" style="padding:0px !important;display:none;">
    <div class="modal-dialog"> 
        <div class="modal-content">
        	<%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true"> 
            	<span><i class="fa fa-remove"></i></span>
            </button>--%>
           
             <div class="form-submit">
               
                                 
                                     <div class="start-heading">
                                            <h2><%=fns.getltext("Create Holiday", "2")%></h2>
                                        </div>
                                        <div class="form-horizontal row-border">
                                       <table  class="table table-bordered">
     <tr><td><%=fns.getltext("Holiday Name", "2")%></td><td><asp:TextBox ID="TextBox3" runat="server" CssClass="input100"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter holiday name"  
         Display="Dynamic" SetFocusOnError="True" ValidationGroup="pnlr2" ControlToValidate="TextBox3"></asp:RequiredFieldValidator>
       
        </td></tr>
       <tr><td><%=fns.getltext("Holiday Detail", "2")%></td><td><asp:TextBox ID="TextBox4" CssClass="input100" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox> </td></tr>

        <tr><td><%=fns.getltext("Date", "2")%></td><td><date:date ID="Date2" runat="server" requeredEnabled="true" requeredDisplay="Dynamic" ValidationGroup="pnpnlr2l2" />  </td></tr>
         
        <tr><td colspan="2">
        <center>
        <asp:Button ID="Button5" runat="server" Text="Save" OnClick="btnaddholiday1_click" CssClass="btn btn-success" ValidationGroup="pnlr2" /> &nbsp;<asp:Button ID="Button6" runat="server" Text="Cancel" CssClass="btn btn-inverse"  OnClick="btncancelholiday1_click"  /> 
        </center>
         </td></tr>


     </table>
                                    
                                
                                         
                              </div>
            </div>
        </div>
    </div>
</div>
    


    

    
<asp:HiddenField ID="hdnstatus" runat="server" />
                                             <asp:HiddenField ID="hdnvalue1" runat="server" />
                                              <asp:HiddenField ID="hdnvalue2" runat="server" />
                                                <asp:HiddenField ID="hdnvalue3" runat="server" />

                                                

<script type="text/javascript">

    if (document.getElementById("<%=hdnstatus.ClientID %>").value == 1) {

        window.parent.document.getElementById("lbladdholidaylistdate").innerHTML = "<%=fns.getltext("Started", "2")%> : " + document.getElementById("<%=hdnvalue1.ClientID %>").value;

        window.parent.document.getElementById("lblholidaystatus").innerHTML = document.getElementById("<%=hdnvalue3.ClientID %>").value;
        window.parent.document.getElementById("lblholidaystatusdate").innerHTML = document.getElementById("<%=hdnvalue2.ClientID %>").value;

        window.parent.document.getElementById("aaddholidaylist").className = "setup-tile inline-block complete";
    }


</script>

    
    <!-- Load site level scripts -->
<script type="text/javascript" src="assets/js/jquery-1.10.2.min.js"></script> 							<!-- Load jQuery -->
<script type="text/javascript" src="assets/js/jqueryui-1.10.3.min.js"></script> 							<!-- Load jQueryUI -->
<script type="text/javascript" src="assets/js/bootstrap.min.js"></script> 								<!-- Load Bootstrap -->
<script type="text/javascript" src="assets/js/enquire.min.js"></script> 									<!-- Load Enquire -->

<script type="text/javascript" src="assets/plugins/velocityjs/velocity.min.js"></script>					<!-- Load Velocity for Animated Content -->
<script type="text/javascript" src="assets/plugins/velocityjs/velocity.ui.min.js"></script>

<script type="text/javascript" src="assets/plugins/wijets/wijets.js"></script>     						<!-- Wijet -->

<script type="text/javascript" src="assets/plugins/codeprettifier/prettify.js"></script> 				<!-- Code Prettifier  -->
<script type="text/javascript" src="assets/plugins/bootstrap-switch/bootstrap-switch.js"></script> 		<!-- Swith/Toggle Button -->

<script type="text/javascript" src="assets/plugins/bootstrap-tabdrop/js/bootstrap-tabdrop.js"></script>  <!-- Bootstrap Tabdrop -->

<script type="text/javascript" src="assets/plugins/iCheck/icheck.min.js"></script>     					<!-- iCheck -->

<script type="text/javascript" src="assets/plugins/nanoScroller/js/jquery.nanoscroller.min.js"></script> <!-- nano scroller -->

<script type="text/javascript" src="assets/js/application.js"></script>
<script type="text/javascript" src="assets/demo/demo.js"></script>
<script type="text/javascript" src="assets/demo/demo-switcher.js"></script>

<!-- End loading site level scripts -->
<script type="text/javascript" src="assets/plugins/smartmenus/jquery.smartmenus.js"></script>
<script type="text/javascript" src="assets/plugins/smartmenus/addons/bootstrap/jquery.smartmenus.bootstrap.js"></script>


  <link href="DateSelector/cal/popcalendar.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="DateSelector/cal/popcalendar.js"></script> 
    </div>

</div>
               

<script src="/pms/assets/js/jquery.dataTables.min.js"></script>
<script>
    const table = new DataTable('.datatable', {  
        scroll:  true,
        scrollCollapse: true,
        paging:         false,
		searching:      true,
    });
</script>
        <script>
function initDataTable() {
    var table = $('#grdholidaylist'); // use your GridView ID
debugger
    // Destroy if already initialized
    if ($.fn.DataTable.isDataTable(table)) {
try
{
        table.DataTable().destroy();
}
catch
{
}
    }

    // Initialize again
    table.DataTable({
        scroll:  true,
        scrollCollapse: true,
        paging:         false,
		searching:      true,
    });
}
$(document).ready(function () {
    initDataTable();
});
if (typeof Sys !== "undefined") {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        initDataTable();
    });
}
        </script>
    </form>
</body>
</html>
