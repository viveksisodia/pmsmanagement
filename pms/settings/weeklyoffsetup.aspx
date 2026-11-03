<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weeklyoffsetup.aspx.cs" Inherits="weeklyoffsetup" %>
<%@ Register Src="~/DateSelector/DateSelector.ascx" TagName="date"  TagPrefix="date" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-touch-fullscreen" content="yes">
    <meta name="author" content="Absolutewebtech">
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

    </style> 


    <script>

        function changechk(val, val1) {
            if (document.getElementById(val).checked) {
                document.getElementById(val1).style.display = "block";
            }
            else {
                document.getElementById(val1).style.display = "none";
            }

        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
         
  
        <div class="static-content-wrapper">
    <div class="static-content">
        <div class="page-content">
            <div class="container">
                <div class="row">           
                    <section class="main" style="padding-top:20px;">
                  
                             <div class="col-sm-12" style="border-left:1px solid #ccc;">
   

    <div class="container">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div id="message" runat="server" style="margin-top:10px; margin-left:40px; font-size:medium; font-weight:bold; color:Green;"  visible="false"></div>

 <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div class="divWaiting" onclick="alert('Please wait data is loading..');"> 
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
 
<asp:Panel ID="pnlholidaylist" runat="server">

   <div class="start-headingtop" style="float:left; text-align:left;">
       <h2><%=fns.getltext("Weekly Break", "2")%></h2>
       <a role="button" data-bs-toggle="modal" data-bs-target="#myModalform4" class="btn btn-primary btn-right" href="javascript:void(0);">
       + <%=fns.getltext("Create", "2")%>

       </a>
   </div>
      <%--<p><%=fns.getltext("Create Week Breaks Program for differnet groups of employees or any specific Employee", "2")%></p>--%>
       

   
       <asp:GridView ID="grduser" runat="server" ShowHeaderWhenEmpty="True"
             AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" 
             DataKeyNames="id"
           
           
             onrowdatabound="grduser_RowDataBound"> 
     
          
 
            <Columns>
 
            <asp:TemplateField >
            <HeaderTemplate>Week Break Program</HeaderTemplate>
            <ItemTemplate>
     <%#Eval("weekoffname")%><br />
               <%#showweekdays(Eval("isweekoff1").ToString(),Eval("isweekoff1type").ToString(),Eval("isweekoff2").ToString(),Eval("isweekoff2type").ToString(),Eval("isweekoff3").ToString(),Eval("isweekoff3type").ToString(),Eval("isweekoff4").ToString(),Eval("isweekoff4type").ToString(),Eval("isweekoff5").ToString(),Eval("isweekoff5type").ToString(),Eval("isweekoff6").ToString(),Eval("isweekoff6type").ToString(),Eval("isweekoff7").ToString(),Eval("isweekoff7type").ToString())%>  
            </ItemTemplate>
            </asp:TemplateField>
 
            <asp:TemplateField>
            <HeaderTemplate>Applied to Whom</HeaderTemplate>
            <ItemTemplate>
           <%#showapplies(Eval("id").ToString()) %>
            </ItemTemplate>
            </asp:TemplateField>
           
           
           
            
             <asp:TemplateField>
            <HeaderTemplate>Click to make Default</HeaderTemplate>
            <ItemTemplate>
           <asp:HiddenField ID="hdndefault" runat="server" Value=<%#Eval("isdefault")%> />

             <asp:LinkButton ID="linksetdefault" runat="server" onclick="linksetdefault_Click">   <image src="/Images/checkmarkdefault.png" title="This is default list" ></asp:LinkButton>
              <asp:LinkButton ID="linksetdefault1" runat="server" onclick="linksetdefault1_Click">   <image src="/Images/checkmarkddefault1.png"  title="Set as default"></asp:LinkButton>
          
            </ItemTemplate>
            </asp:TemplateField>
            
            
 
            <asp:TemplateField>
            <HeaderTemplate>Operation</HeaderTemplate>
            <ItemTemplate>         
                   &nbsp;&nbsp;
                  
            <asp:LinkButton ID="lnkedit" runat="server" onclick="lnkedit_Click"><span class="btn btn-success btn-sm"><i class="fa fa-edit"></i></span></asp:LinkButton>&nbsp; &nbsp;
         
            </ItemTemplate>
            
                
            </asp:TemplateField> 
                      
            </Columns>
            <EmptyDataTemplate>
                      <%=fns.getltext("No record available", "2")%>                    
            </EmptyDataTemplate>       
        </asp:GridView>


                                        

 
  
  </asp:Panel>



   <div class="modal fade in inner-modal" runat="server" id="myModalform4" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" >
    <div class="modal-dialog"> 
        <div class="modal-content">
     
            
             <div class="form-submit">
               
                                 
                                     <div class="start-heading" style="margin-bottom: 0px !important;">
                                            <h2><%=fns.getltext("Initialise Week Breaks Program", "2")%></h2>
                                            
                                          <%--  <br />
                                            You can specify weekly off schedules here. These can be assigned to individual employees. The default weekly off plan gets applied to all employees when not explicitly set.
                                      --%>
                                        </div>
                                     
                                       <table class="table">
     <tr><td style="width:100px;"><%=fns.getltext("Program Name", "2")%></td><td  colspan="2"><asp:TextBox ID="txtlistname" runat="server"></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Put Program Name"  Display="Dynamic" SetFocusOnError="True" ValidationGroup="pnl1" ControlToValidate="txtlistname"></asp:RequiredFieldValidator>
       
        </td></tr>
       
       <tr ><td rowspan='7'><%=fns.getltext("Choose Days", "2")%></td>
      <td style="vertical-align:middle;"><asp:CheckBox ID="chk1" runat="server" /> <%=fns.getltext("Sunday", "2")%> </td>
       <td style="padding:5px;">
       <div id="div1" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl1" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
       
       </div>
       </td>
       </tr>
       
        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk2" runat="server"  /> <%=fns.getltext("Monday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div2" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl2" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList>
       </div>
        </td>
       </tr>

        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk3" runat="server" /> <%=fns.getltext("Tuesday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div3" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl3" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
       </div>
       </td>
       </tr>

        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk4" runat="server" /> <%=fns.getltext("Wednesday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div4" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl4" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
       </div>
       </td>
       </tr>

        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk5" runat="server" /> <%=fns.getltext("Thrusday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div5" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl5" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
       </div>
       </td>
       </tr>

        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk6" runat="server"  /> <%=fns.getltext("Friday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div6" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl6" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
       </div>
       </td>
       </tr>

        <tr><td style="vertical-align:middle;"><asp:CheckBox ID="chk7" runat="server" /> <%=fns.getltext("Saturday", "2")%> </td>
       <td style="padding:5px;">
        <div id="div7" runat="server" style="display:none;">
       <asp:DropDownList ID="ddl7" runat="server">
       <asp:ListItem Value="1">All</asp:ListItem>
       <asp:ListItem Value="2">1st</asp:ListItem>
       <asp:ListItem Value="3">2nd</asp:ListItem>
       <asp:ListItem Value="4">3rd</asp:ListItem>
       <asp:ListItem Value="5">4th</asp:ListItem>
       <asp:ListItem Value="6">5th</asp:ListItem>
        <asp:ListItem Value="7">Odd(1,3)</asp:ListItem>
       <asp:ListItem Value="8">Odd(1,3,5)</asp:ListItem>
       <asp:ListItem Value="9">Even(2,4)</asp:ListItem>
       </asp:DropDownList> 
      </div>
       </td>
       </tr>
       
     
        <tr><td colspan="3">
        <center>
        <asp:Button ID="btnadd" runat="server" Text="Save" OnClick="btnadd_click" CssClass="btn btn-success"  ValidationGroup="pnl1" /> 
        &nbsp;<asp:Button ID="Button1" runat="server" CssClass="btn btn-inverse" Text="Cancel" OnClick="btncancel_click"  /> 
        </center>
         </td></tr>
     </table>
                                    
                                
                             
            </div>
        </div>
    </div>
</div>


    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnstatus" runat="server" />
                                             <asp:HiddenField ID="hdnvalue1" runat="server" />
                                              <asp:HiddenField ID="hdnvalue2" runat="server" />
                                                <asp:HiddenField ID="hdnvalue3" runat="server" />

                                                

<script type="text/javascript">

    if (document.getElementById("<%=hdnstatus.ClientID %>").value == 1) {

        window.parent.document.getElementById("lbladdweekoffdate").innerHTML = "Started : " + document.getElementById("<%=hdnvalue1.ClientID %>").value;

        window.parent.document.getElementById("lblweekoffstatus").innerHTML = document.getElementById("<%=hdnvalue3.ClientID %>").value;
        window.parent.document.getElementById("lblweekoffstatusdate").innerHTML = document.getElementById("<%=hdnvalue2.ClientID %>").value;

        window.parent.document.getElementById("aaadweekoff").className = "setup-tile inline-block complete";
    }


</script>

  
    
    <!-- Load site level scripts -->
         <script src="/pms/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
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


  <link href="/DateSelector/cal/popcalendar.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="/DateSelector/cal/popcalendar.js"></script> 
    </div>
                                 </div>
                                  </section>
               <%-- </div>--%>
            </div> <!-- .container-fluid -->
        </div> <!-- #page-content -->
    </div>
</div>

    </form>
</body>
</html>
