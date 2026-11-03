<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValidateDocument.aspx.cs" Inherits="employee_lettertemplate" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate" Assembly="GleamTech.DocumentUltimate" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate.AspNet.WebForms" Assembly="GleamTech.DocumentUltimate" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="apple-touch-fullscreen" content="yes">
<meta name="description" content="">
<meta name="author" content="">
<link type='text/css' href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400italic,600' rel='stylesheet'>

    <link type="text/css" href="assets/fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet">        <!-- Font Awesome -->
    <link type="text/css" href="assets/fonts/themify-icons/themify-icons.css" rel="stylesheet">              <!-- Themify Icons -->
    <link type="text/css" href="assets/css/styles.css" rel="stylesheet">                                     <!-- Core CSS with all styles -->

    <link type="text/css" href="assets/plugins/codeprettifier/prettify.css" rel="stylesheet">                <!-- Code Prettifier -->
    <link type="text/css" href="assets/plugins/iCheck/skins/minimal/blue.css" rel="stylesheet"> 
    <link type="text/css" href="assets/plugins/datatables/dataTables.bootstrap.css" rel="stylesheet">        <!-- Bootstrap Support for Datatables -->
<link type="text/css" href="assets/plugins/datatables/dataTables.themify.css" rel="stylesheet">  
<style>
    .glyphicon-file-pdf,
.glyphicon-download,
[class*="download"] {
    display: none !important;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
      
        <section class="main">
              <div class="static-content">
                <div class="page-content">
                  <div class="container">
                    <div data-widget-group="group1">
                      <div class="row">
                     
                     <asp:Panel ID="pnl1" runat="server">
                     
                     
                     
                        <div class="steps text-left">
                     
                     
                          <div class="step-wrapper">
                          
                          
                                            
                         
                         
                          </div>
                        </div>
                        <div class="container">
                            <div class="col-sm-12 apply-plane">
                          
                           <div class="form-group">
                            	<div class="col-sm-8" style="margin-bottom:10px;">
                          
                  <asp:UpdatePanel ID="updapanl1" runat="server">
                    <ContentTemplate>


                         
                                    
<GleamTech:DocumentViewerControl runat="server"  ID="docview" DownloadUrl="false" DownloadEnabled="false"   
     
           
             />
                                   
                              


  </ContentTemplate>
            </asp:UpdatePanel>                                


     </div>
                            

                                

                           <div class="col-md-4 col-sm-4 new-letter-wrapper">
                                	
                    <p><asp:Label ID="docdetails" runat="server"></asp:Label></p>
               
                              <asp:Button id="btnsave" runat="server" Text="Approve" OnClick="btnsave_click" CssClass="btn btn-success" />   
                               &nbsp;
                                <asp:Button id="btnreject" runat="server" Text="Reject" OnClick="btnreject_click" CssClass="btn btn-danger" />   <br /><br/>
                                    <div style="background-color:#d9edf7; padding:8px;">

                                    Note: Use top small arrows to move from one page to another in a single document. To switch to the next/previous document click on right/left arrow respectively 

                                </div> 
                                </div>


                           
</div>

                        </div>

                         
                              




                              
                          
                        </div>
                      
                      </asp:Panel>
                      
                    
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          <div style="clear:both;"></div>
        </section>
        

   <div class="modal fade in inner-modal" id="myModalform6" role="dialog" aria-labelledby="myModalLabel" style="display:none;">
       <br /> <br /> <br />

  <div class="modal-dialog">
    <div class="modal-content">
     
    <a href='javascript://' class="close" onclick="hiddiv()" style="color:White;position: absolute;top: 10px;right: 10px;z-index: 9999;">X</a>
        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar">
            <h5><asp:Label ID="lblheading" runat="server" Text=""></asp:Label>
            <asp:HiddenField ID="hdnempid" runat="server" />

           
            </h5>
        </div>
        </header>
        
        <br /><br /><br />

          <iframe id="addModalsrc"  width="100%"  style="padding:0px 15px;"></iframe>
         </div>
         </div>
         </div>






    
  <%--  <script type="text/javascript" src="assets/js/jquery-1.10.2.min.js"></script> 							<!-- Load jQuery -->
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
<script type="text/javascript" src="assets/demo/demo-switcher.js"></script>--%>

<!-- End loading site level scripts -->
    
    <!-- Load page level scripts-->
    
<script type="text/javascript" src="assets/plugins/datatables/jquery.dataTables.js"></script>                            <!-- Data Tables -->
<script type="text/javascript" src="assets/plugins/datatables/TableTools.js"></script>                                   <!-- TableTools -->
<script type="text/javascript" src="assets/plugins/jquery-editable/jquery.editable.js"></script>                         <!-- jQuery Editable -->
<script type="text/javascript" src="assets/plugins/datatables/dataTables.editor.js"></script>                            <!-- Data Tables Editor-->
<script type="text/javascript" src="assets/plugins/datatables/dataTables.editor.bootstrap.js"></script>                  <!-- Data Tables Editor for Bootstrap-->
<script type="text/javascript" src="assets/plugins/datatables/dataTables.bootstrap.js"></script>                         <!-- Bootstrap Support for Datatables -->

<script type="text/javascript" src="assets/demo/demo-tableeditable.js"></script> 	


        
   
 <script>
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();   
});
</script>
        <script>

function hidePdfButtons() {
    // Try normal DOM
    document.querySelectorAll('.glyphicon-file-pdf, [id*=downloadAsPdf]').forEach(function (el) {
        el.style.display = 'none';
    });

    // Try inside iframe (VERY IMPORTANT)
    document.querySelectorAll('iframe').forEach(function (iframe) {
        try {
            var doc = iframe.contentDocument || iframe.contentWindow.document;
            if (doc) {
                doc.querySelectorAll('.glyphicon-file-pdf, [id*=downloadAsPdf]').forEach(function (el) {
                    el.style.display = 'none';
                });
            }
        } catch (e) {
            // cross-origin - ignore
        }
    });
}

// Run continuously (because viewer re-renders toolbar)
setInterval(hidePdfButtons, 500);

</script>
    </form>
</body>
</html>
