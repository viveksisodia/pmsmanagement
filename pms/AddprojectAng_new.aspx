<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="AddprojectAng_new.aspx.cs" Inherits="AddprojectAng" ValidateRequest="false" %>
 
<!DOCTYPE html>
<html>
    <head>
   <script src="/pms/angularmaster/js/angular.min.js"></script>
<script src="/pms/angularmaster/js/angular-sanitize.js"></script>
<script src="/pms/angularmaster/js/dirPagination.js"></script>   
<script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script> 
<script src="/pms/projects/app/mproject.main.js?v=<%=DateTime.Now.Ticks %>"></script> 

   
 </head>
 
   
    <body>
                        
                       
                      <div class="customer_wrapper customer_form"  ng-app="PMSerp"   ng-controller="mprojectCtrl">

    {{projectdata.ProjectName}}
    {{projectdata.ClientName}}

</div>
     
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script>                   
           
</body>

   
 </html>


