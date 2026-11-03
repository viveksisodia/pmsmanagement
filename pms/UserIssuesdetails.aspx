<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserIssuesdetails.aspx.cs" Inherits="UserIssuesdetails" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
      <script src="/pms/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="/pms/assets/libs/simplebar/simplebar.min.js"></script>
    <script src="/pms/assets/js/plugins.js"></script>
    <script src="/pms/assets/libs/prismjs/prism.js"></script>
    <script src="/pms/assets/js/app.js"></script>  
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="card-body">
                                 <div class="table-responsive">
                                  <div id="divgridview" style="width:100%;height:450px">
                                 <asp:Label ID="lblissuesdetails" runat="server"></asp:Label>
                                 </div>
                             
        </div>
    </div>
     <asp:HiddenField ID="hdissuesid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid)
        {
            debugger

                   var issuesid=document.getElementById("<%=hdissuesid.ClientID%>").value;
                    //alert(issuesid);
                    $.ajax({
                    url: '/pms/Addissues.aspx/Update_Status',
                    data: "{ 'issuesid': '" + issuesid + "' ,'statusid': '" + statusid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdt(response) {
debugger
            alert('Data Saved successfully.');
          var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
         function setissustatusid(issuesid,btnststusid)
        {
            debugger
             document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdissuesid.ClientID%>").value = issuesid;
        }
            </script>
       <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</form>
    </body>
    </html>

