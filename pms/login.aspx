<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=no" />
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Absolute ERP</title>
<link rel="shortcut icon" href="/pms/assets/images/favicon.png">
<link href="/pms/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<link href="/pms/assets/css/icons.min.css" rel="stylesheet" type="text/css" />
<link href="/pms/assets/css/app.min.css" rel="stylesheet" type="text/css" />
<style>
.panel-heading h2 { text-align:left; color:#450c3a; background:#ebeae7; width:300px; margin:0px; padding:10px 16px; font-size:16px; }
.btn-primary {
	color: #fff;
	background-color: #670955;
	border-color: #450c3a;
	width:100% !important;
	padding:7px 10px !important;
}
.btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open .dropdown-toggle.btn-primary {
	color: #fff;
	background-color: #450c3a;
	border-color: #670955
}
.input-group {
  position: relative;
  display: table;
  border-collapse: separate;
  width: 100%;
}
.input-group[class*="col-"] {
  float: none;
  padding-left: 0;
  padding-right: 0;
}
.input-group .form-control {
  position: relative;
  z-index: 2;
  float: left;
  width: 100%;
  margin-bottom: 0;
  min-height: 22px;
  border-radius:0;
}
.input-group-lg > .form-control,
.input-group-lg > .input-group-addon,
.input-group-lg > .input-group-btn > .btn {
  padding: 6px 12px;
  border-radius: 2px;
  padding: 6px 10px;
  font-size: 16px;
  height: 40px;
  line-height: 1.65;
}
select.input-group-lg > .form-control,
select.input-group-lg > .input-group-addon,
select.input-group-lg > .input-group-btn > .btn {
  height: 40px;
  line-height: 40px;
}
textarea.input-group-lg > .form-control,
textarea.input-group-lg > .input-group-addon,
textarea.input-group-lg > .input-group-btn > .btn,
select[multiple].input-group-lg > .form-control,
select[multiple].input-group-lg > .input-group-addon,
select[multiple].input-group-lg > .input-group-btn > .btn {
  height: auto;
}
.input-group-sm > .form-control,
.input-group-sm > .input-group-addon,
.input-group-sm > .input-group-btn > .btn {
  padding: 3px 6px;
  font-size: 12px;
  border-radius: 2px;
  padding: 4px 4px;
  height: 24px;
  line-height: 1.35;
  
}
select.input-group-sm > .form-control,
select.input-group-sm > .input-group-addon,
select.input-group-sm > .input-group-btn > .btn {
  height: 24px;
  line-height: 24px;
}
textarea.input-group-sm > .form-control,
textarea.input-group-sm > .input-group-addon,
textarea.input-group-sm > .input-group-btn > .btn,
select[multiple].input-group-sm > .form-control,
select[multiple].input-group-sm > .input-group-addon,
select[multiple].input-group-sm > .input-group-btn > .btn {
  height: auto;
}
.input-group-addon,
.input-group-btn,
.input-group .form-control {
  display: table-cell;
}
.input-group-addon:not(:first-child):not(:last-child),
.input-group-btn:not(:first-child):not(:last-child),
.input-group .form-control:not(:first-child):not(:last-child) {
  border-radius: 0;
}
.input-group-addon,
.input-group-btn {
  width: 1%;
  white-space: nowrap;
  vertical-align: middle;
}
.input-group-addon {
  padding: 5px 12px;
  font-size: 18px;
  font-weight: normal;
  line-height: 1;
  color: #212121;
  text-align: center;
  background-color: #e0e0e0;
  border: 1px solid #e0e0e0;
  border-radius: 2px;
  width: 16px;
}
.input-group-addon.input-sm {
  padding: 3px 6px;
  font-size: 12px;
  border-radius: 2px;
}
.input-group-addon.input-lg {
  padding: 6px 12px;
  font-size: 16px;
  border-radius: 2px;
}
.input-group-addon input[type="radio"],
.input-group-addon input[type="checkbox"] {
  margin-top: 0;
}
.input-group .form-control:first-child,
.input-group-addon:first-child,
.input-group-btn:first-child > .btn,
.input-group-btn:first-child > .btn-group > .btn,
.input-group-btn:first-child > .dropdown-toggle,
.input-group-btn:last-child > .btn:not(:last-child):not(.dropdown-toggle),
.input-group-btn:last-child > .btn-group:not(:last-child) > .btn {
  border-bottom-right-radius: 0;
  border-top-right-radius: 0;
}
.input-group-addon:first-child {
  border-right: 0;
}
.input-group .form-control:last-child,
.input-group-addon:last-child,
.input-group-btn:last-child > .btn,
.input-group-btn:last-child > .btn-group > .btn,
.input-group-btn:last-child > .dropdown-toggle,
.input-group-btn:first-child > .btn:not(:first-child),
.input-group-btn:first-child > .btn-group:not(:first-child) > .btn {
  border-bottom-left-radius: 0;
  border-top-left-radius: 0;
}
.input-group-addon:last-child {
  border-left: 0;
}
.input-group-btn {
  position: relative;
  font-size: 0;
  white-space: nowrap;
}
.input-group-btn > .btn {
  position: relative;
}
.input-group-btn > .btn + .btn {
  margin-left: -1px;
}
.input-group-btn > .btn:hover,
.input-group-btn > .btn:focus,
.input-group-btn > .btn:active {
  z-index: 2;
}
.input-group-btn:first-child > .btn,
.input-group-btn:first-child > .btn-group {
  margin-right: -1px;
}
.input-group-btn:last-child > .btn,
.input-group-btn:last-child > .btn-group {
  margin-left: -1px;
}
.forgot-pass { background:none; font-size: 14px;font-weight: normal !important;padding: 0px !important;text-transform: none !important;margin-top: 5px;color: #fff; float: right; }
.partner-logo { width:100%; position:absolute; top:50px; left:0px; text-align: left; }
.partner-logo img { margin-left:65px; }
@media (max-width:600px) {
    .partner-logo { text-align:center; top:30px; }
    .partner-logo img { margin-left:0px; height:100px; }
	div#header { padding: 150px 0 0 0; }
}
div#header {
  padding: 80px 0 0 0;
}
#content { width:490px; padding:15px !important; background: rgba(0,0,0,0.2) !important; border: 1px solid #999 !important; }
@media (max-width: 500px) {
  #content {
    width: 80%;
  }
  .input-group .form-control {
  width: 100% !important;
}
.input-group input[type="submit"] { width:100% !important; }
}
</style>
</head>
<body style="background-image: url(assets/images/bg.jpg);background-position: center center;background-repeat: no-repeat;background-attachment: fixed;background-size: cover;background-color: #fff;">
<form id="f1" runat="server">
	<center>
	
	<!-- HEADER -->
	<div id="header">
		
		<div class="page-full-width cf" style="margin-bottom:25px;">
            <img src="/pms/assets/images/logo-dark.png" width="250px" alt="Absolute ERP" />
		</div> 
 
	</div> 
	

	 <asp:Panel ID="pnllogin" runat="server">
	<!-- MAIN CONTENT -->
    
	<div id="content">
		

      



		<fieldset>
        		<p> <asp:Label ID="lblmsg" runat="server"></asp:Label></p>
                <div class="form-group mb-md" style="margin-top: -10px;">
                    <div class="col-xs-12">
                        <div class="input-group">							
                            <span class="input-group-addon">
                                <i class="ti ti-user"></i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="gi" Text="*"
                               ControlToValidate="uidtxt" runat="server" ErrorMessage="Please Enter username !!!" Display="Dynamic"></asp:RequiredFieldValidator>
                            </span>
                            <asp:TextBox ID="uidtxt" runat="server" CssClass="form-control" placeholder="username"></asp:TextBox>

                              

                               

                        </div>
                    </div>
                </div>
                
                <div class="form-group mb-md" style="margin-top:10px;">
                    <div class="col-xs-12">
                        <div class="input-group">							
                            <span class="input-group-addon">
                                <i class="ti ti-lock"></i>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="gi" Text="*" ControlToValidate="pwdtxt" runat="server" ErrorMessage="Please Enter Password !!!" Display="Dynamic"></asp:RequiredFieldValidator>
                            </span>
                            <asp:TextBox ID="pwdtxt" runat="server" CssClass="form-control" placeholder="password" TextMode="Password"></asp:TextBox>
                            

                        </div>
                    </div>
                </div>	
                <div class="form-group mb-md" style="margin-bottom:0px !important; margin-top:10px !important;">
                    <div class="col-xs-12">

                  

                  <%--  	<a href="" style="font-size:12px; float: left; margin-bottom:5px;"></a>--%>
                        <div class="input-group" style="width:100%;">
                            <asp:Button ID="submit" runat="server" Text="LOG IN" CssClass="form-control btn-primary" OnClick="submit_click"  ValidationGroup="gi" />
                            <div style="display:none;">
                            <asp:Button ID="forgotpwdlink1" runat="server" CssClass="forgot-pass" OnClick="forgotpwdlink_click"  Text="Forgot Password?" />
                            </div>
                       <%--  <asp:LinkButton ID="forgotpwdlink" runat="server" Text="Forgot password?" OnClick="forgotpwdlink_click" CausesValidation="false" style="font-size:12px; float: left; margin-bottom:5px;"></asp:LinkButton>
                      --%> 
                        </div>
                    </div>
                </div>			
			
 
			</fieldset>

            	</div> 

 </asp:Panel>


		
		

	
	 <asp:Panel ID="pnlforgotpwd" runat="server" Visible="false">
  <div class="panel-heading">
            <h2>Password Reset Request</h2>
        </div>
	<div id="content" style="width:300px; padding:15px;">
 
 <fieldset>
        		<p> <asp:Label ID="lblmsgchangepwd" runat="server"></asp:Label></p>
                <div class="form-group mb-md" style="margin-top: -10px;">
                    <div class="col-xs-12">
                        <div class="input-group">							
                            <span class="input-group-addon">
                                <i class="fa fa-user"></i><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="gifor" Text="*"
                               ControlToValidate="txtemailid" runat="server" ErrorMessage="Please Enter txtemailid !!!" Display="Dynamic"></asp:RequiredFieldValidator>
                            </span>
                            <asp:TextBox ID="txtemailid" runat="server" CssClass="form-control" placeholder="Email-ID"></asp:TextBox>

                              

                               

                        </div>
                    </div>
                </div>
                
              
                <div class="form-group mb-md" style="margin-bottom:0px !important; margin-top:10px !important;">
                    <div class="col-xs-12">
                    
                        <div class="input-group" style="width:100%;">
                            <asp:Button ID="Button1" runat="server" Text="Request" CssClass="form-control btn-primary" OnClick="resetpwd"  ValidationGroup="gifor" />
                            	<a href="" class="forgot-pass">Back to Login</a>


                        </div>
                    </div>
                </div>			
			
 
			</fieldset>

            	</div> 

 
 </asp:Panel>
	

     <asp:Panel ID="pnlsendreques" runat="server" Visible="false">
  <div class="panel-heading">
            <h2>Password Reset Request</h2>
        </div>
	<div id="content" style="width:300px; padding:15px;">
 
 <fieldset>

 <asp:Label ID="lblmsgchangepwdreques" runat="server" ></asp:Label><br />
 	<a href="" style="font-size:12px; float: left; margin-bottom:5px;">Back to login</a>
 </fieldset>
 </div>
 </asp:Panel>



	<!-- FOOTER -->
        
	<div id="footer" class="mt-4">
		<p style="color:#000 !important;background:#ccc;display: inline-block;padding: 2px 8px;margin-bottom: 4px;font-size:13px;">Copyright © <a href="www.erpabsolute.com" target="_blank" style="color:#277fd9;">Absolute ERP</a> | All Rights Reserved.</p>
	</div> <!-- end footer -->
	</center>
</form>
</body>
</html>
