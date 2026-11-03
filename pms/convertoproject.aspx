<%@ Page Language="C#" AutoEventWireup="true" CodeFile="convertoproject.aspx.cs" Inherits="convertoproject" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="shortcut icon" href="/crm/assets/images/favicon.png">
    <link rel="stylesheet" href="/crm/assets/libs/dragula/dragula.min.css">
    <link href="/crm/assets/js/jquery.dataTables.min.css" rel="stylesheet" type="text/css">
    <!-- Sweet Alert css-->
    <link href="/crm/assets/libs/sweetalert2/sweetalert2.min.css" rel="stylesheet" type="text/css">
    <!-- Layout config Js -->
    <script src="/crm/assets/js/layout.js"></script>
    <!-- Bootstrap Css -->
    <link href="/crm/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <!-- Icons Css -->
    <link href="/crm/assets/css/icons.min.css" rel="stylesheet" type="text/css">
    <!-- App Css-->
    <link href="/crm/assets/css/app.min.css" rel="stylesheet" type="text/css">
    <!-- custom Css-->
    <link href="/crm/assets/css/custom.min.css" rel="stylesheet" type="text/css">
    <script src="../DateSelector/cal/popcalendar.js"></script>
    <link href="../DateSelector/cal/popcalendar.css" rel="stylesheet" />
     <%if (isthemeid == 2)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color2.css" />

    <%} %>
    <%if (isthemeid == 3)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color3.css" />

    <%} %>
    <%if (isthemeid == 4)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color4.css" />

    <%} %>
    <%if (isthemeid == 5)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color5.css" />

    <%} %>
      <%if (isthemeid == 6)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color6.css" />

    <%} %>
      <%if (isthemeid == 7)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color7.css" />

    <%} %>
       <%if (isthemeid == 8)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color8.css" />

    <%} %>
       <%if (isthemeid == 9)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color9.css" />

    <%} %>
        <%if (isthemeid ==10)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color10.css" />

    <%} %>

     <%if (isthemeid ==11)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color11.css" />

    <%} %>


     <%if (isthemeid ==12)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color12.css" />

    <%} %>
    <%if (isthemeid ==13)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color13.css" />

    <%} %>
    <%if (isthemeid ==14)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color14.css" />

    <%} %>
    <%if (isthemeid ==15)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color15.css" />

    <%} %>
    <%if (isthemeid ==16)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color16.css" />

    <%} %>
    <%if (isthemeid ==17)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color17.css" />

    <%} %>
    <%if (isthemeid ==18)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color18.css" />

    <%} %>
    <%if (isthemeid ==19)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color19.css" />

    <%} %>
    <%if (isthemeid ==20)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color20.css" />

    <%} %>
    <%if (isthemeid ==21)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color21.css" />

    <%} %>
    <%if (isthemeid ==22)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color22.css" />

    <%} %>
    <%if (isthemeid ==23)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color23.css" />

    <%} %>
    <%if (isthemeid ==24)
       { %>

    <link rel="stylesheet" type="text/css"  href="/crm/assets/css/color24.css" />

    <%} %>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       
 <script type="text/javascript">
                                                 function showdivimglead4() {
                                                     if (Page_ClientValidate("button")) {
                                                         if (confirm('Do you want Leave this page ?')) {
                                                             document.getElementById("btncloselead4").click();
                                                             return true;
                                                         }
                                                         else {
                                                             return false;
                                                         }
                                                     }
                                                 }
            </script>
        <%-- <script src="assets/js/jquery-1.10.2.min.js"></script>--%>
<script src="https://code.jquery.com/ui/1.13.3/jquery-ui.js"></script>
        <script type="text/javascript">
             function bindmaxsize(ctrl)
      {
          //alert(ctrl);
        try {
            bindmaxsizedetails();
              searchdata();
          }
          catch
          {}
      }
        // JavaScript function to handle the change event of the first dropdown
            function bindmaxsizedetails() {
                 $('#<%= ddlmaxsize.ClientID %>').empty();
                debugger
                if (document.getElementById("<%=ddlminsize.ClientID%>").value == "") {
                   // alert
                    return false;
                }
                if (document.getElementById("<%=hdnbindtype.ClientID%>").value == "") {
                   // alert
                    return false;
                }
                var typfor = "2";
                try {
                    typfor = document.getElementById("<%=hdnbindtype.ClientID%>").value;
                }
                catch
                {
             typfor = "2";
                }
            var selectedValue = document.getElementById("<%=ddlminsize.ClientID%>").value; // Get the selected value from the first dropdown
                 $.ajax({
            url: '<%=ResolveUrl("/crm/AddPropertyLeads.aspx/Getmaxsize") %>',
            data: "{ 'minsize': '" + selectedValue + "','typefor':'"+typfor+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                         debugger
                 //alert("ok");
                    $('#<%= ddlmaxsize.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%= ddlmaxsize.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
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
             function bindminsizedetails() {
                debugger
               
                var typfor = "2";
                try {
                    typfor = document.getElementById("<%=hdnbindtype.ClientID%>").value;
                }
                catch
                {
             typfor = "2";
                }
            
                 $.ajax({
            url: '<%=ResolveUrl("/crm/AddPropertyLeads.aspx/Getminsize") %>',
            data: "{'typefor':'"+typfor+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                         debugger
                 //alert("ok");
                    $('#<%= ddlminsize.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%= ddlminsize.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
                         });
                         bindmaxsizedetails();
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
             function bindmaxbudget(ctrl)
      {
          //alert(ctrl);
          try {
              <%--document.getElementById("<%=ButtonleadB.ClientID%>").click();--%>
              bindmaxbudgetdetails();
              searchdata();
          }
          catch
          {}
      }
            function bindmaxbudgetdetails() {
                debugger
                 $('#<%= ddlmaxBudget.ClientID %>').empty();
                if (document.getElementById("<%=ddlminBudget.ClientID%>").value == "") {
                   // alert
                    return false;
                }
                if (document.getElementById("<%=hdnbindtype.ClientID%>").value == "") {
                   // alert
                    return false;
                }
                var typfor = "2";
                try {
                    typfor = document.getElementById("<%=hdnbindtype.ClientID%>").value;
                }
                catch
                {
             typfor = "2";
                }
            var selectedValue = document.getElementById("<%=ddlminBudget.ClientID%>").value; // Get the selected value from the first dropdown
                 $.ajax({
            url: '<%=ResolveUrl("/crm/AddPropertyLeads.aspx/Getmaxbudget") %>',
            data: "{ 'minsize': '" + selectedValue + "','typefor':'"+typfor+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                 //alert("ok");
                    $('#<%= ddlmaxBudget.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%= ddlmaxBudget.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
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
            function bindminbudgetdetails() {
                debugger
               
                var typfor = "2";
                try {
                    typfor = document.getElementById("<%=hdnbindtype.ClientID%>").value;
                }
                catch
                {
              typfor = "2";
                }
            
                 $.ajax({
            url: '<%=ResolveUrl("/crm/AddPropertyLeads.aspx/Getminbudget") %>',
            data: "{'typefor':'"+typfor+"'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
                     success: function (response) {
                 //alert("ok");
                    $('#<%= ddlminBudget.ClientID %>').empty();

                    // Add new options to the second dropdown
                    $.each(response.d, function(index, item) {
                        $('#<%= ddlminBudget.ClientID %>').append($('<option>', { 
                            value: item.Value, 
                            text: item.Text 
                        }));
                         });
                         bindmaxbudgetdetails();
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
    </script>
  <script>
  function AddNewSource() {
                //debugger
                document.getElementById('<%=hdnnewsource.ClientID%>').value = "0";
                document.getElementById("dvnewsource").style.display = "none";
                if (document.getElementById('<%=LeadSource.ClientID%>').value == "-1") {
                    document.getElementById('<%=hdnnewsource.ClientID%>').value = "1";
                    document.getElementById("dvnewsource").style.display = "block";
                    document.getElementById('<%=txtnewsource.ClientID%>').value = "";
                }
                else {

                    document.getElementById('<%=txtnewsource.ClientID%>').value = "";
                }
            }
    $(function () {

        if (document.getElementById("<%=hdnleadeditid.ClientID%>").value == "" || document.getElementById("<%=hdnleadeditid.ClientID%>").value == "0") {
            window.parent.document.getElementById('offcanvasRightLabelpp').innerHTML = "Add New Lead";
        }
       

        <%-- $("#<%= ListDate.ClientID %>").datepicker({
       todayBtn: "linked",
       language: "it",
       autoclose: true,
       todayHighlight: true,
       dateFormat: 'dd/mm/yy' --%>
   });


 

     
   

  </script>
        <script>
            function changetxtrentbuy(val, val1) {
                debugger
                    let lbl = document.getElementById("<%=lblrentbuy.ClientID%>");  
                    lbl.innerHTML = val;
                    document.getElementById("<%=hdisrenbuy.ClientID%>").value = val1;
                document.getElementById("<%=hdnbindtype.ClientID%>").value = val1;
                try {
                    bindminsizedetails();
                    bindmaxsizedetails();
                }
                catch{ }
                try {
                    bindminbudgetdetails();
                    bindmaxbudgetdetails();
                }
                catch{}
                    <%--try {
                        document.getElementById("<%=btnbind.ClientID%>").click();
                    }
                    catch
                    {

                    }--%>
                    searchdata();
                }
                  function changetxtResidentialCommercial(val,val1) {
                      let lbl = document.getElementById("<%=lblResidential.ClientID%>"); 
                      lbl.innerHTML = val;
                      document.getElementById("<%=hdisResidential.ClientID%>").value = val1; 
                        searchdata();
                }
                function changemasterhdnrent() {


                    document.getElementById("<%=hdnrent.ClientID%>").value = val1;  


                }
                function changemastersearchval() {
                     
                }

    const myList1 = [];
    const myList6 = [];
    function Binddetails_listingdata(RecordId, checkboxId, Recordfor)
    {
       
               
        debugger
        //alert(checkboxId);
        if (Recordfor == "1") {
             if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnbuyid.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList1.length = 0;
                myList1.push(...filteredValues);
        }
            const checkbox = $('#' + checkboxId);
            if (checkbox.is(':checked')) {
                if (!myList1.includes(RecordId)) {
                    myList1.push(RecordId);
                } else {

                }
            } else {
                var chkid = "";
                chkid =  RecordId.toString();
                const index = myList1.indexOf(chkid);
                if (index > -1) {
                    myList1.splice(index, 1);
                }
            }
            $('#<%=hdnbuy.ClientID%>').val(myList1.join(','));
            document.getElementById("<%=hdnbuyid.ClientID%>").value = ($('#<%=hdnbuy.ClientID%>').val());
        }
        if (Recordfor == "2")
        {
            if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnrentid.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList6.length = 0;
                myList6.push(...filteredValues);
             }
           
            const checkbox = $('#' + checkboxId);
            if (checkbox.is(':checked')) {
                if (!myList6.includes(RecordId)) {
                    myList6.push(RecordId);
                } else {

                }
            } else {
                var chkid = "";
                chkid =  RecordId.toString();
                const index = myList6.indexOf(chkid);
                if (index > -1) {
                    myList6.splice(index, 1);
                }
            }
              $('#<%=hdnrent.ClientID%>').val(myList6.join(','));
            document.getElementById("<%=hdnrentid.ClientID%>").value = ($('#<%=hdnrent.ClientID%>').val());
          
        }

        searchdata();

            }
            const myList = [];
    const myList2 = [];
      const myList5 = [];
    function Binddetails_listingstatus(RecordId, Recordfor, Recordtype, checkboxId, Feature) { 

        if (Recordfor == "13") { 
             if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnlistingbeds.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList.length = 0;
                myList.push(...filteredValues);
             }
            const checkbox11 = $('#' + checkboxId);

            if (checkbox11.is(':checked')) {

                if (!myList.includes(RecordId)) {
                    myList.push(RecordId);
                } 
            } else {
                 var chkid = "";
                chkid =  RecordId.toString();
                const index = myList.indexOf(chkid);
                if (index > -1) {
                    myList.splice(index, 1);
                }
            }
            $('#<%=hdnlistingbeds.ClientID%>').val(myList.join(','));
            document.getElementById("<%=hdnlistingbeds.ClientID%>").value = ($('#<%=hdnlistingbeds.ClientID%>').val());

        }
        
        if (Recordfor == "14") { 
            if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnlistingBaths.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList2.length = 0;
                myList2.push(...filteredValues);
             }
            const checkbox11 = $('#' + checkboxId);

            if (checkbox11.is(':checked')) {

                if (!myList2.includes(RecordId)) {
                    myList2.push(RecordId);
                }  
            } else {
                var chkid = "";
                chkid =  RecordId.toString();
                const index = myList2.indexOf(chkid);
                if (index > -1) {
                    myList2.splice(index, 1);
                }
            }
            $('#<%=hdnlistingBaths.ClientID%>').val(myList2.join(','));
            document.getElementById("<%=hdnlistingBaths.ClientID%>").value = ($('#<%=hdnlistingBaths.ClientID%>').val());

        }
        if (Recordfor == "15") { 
             if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnlistingstatus.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList5.length = 0;
                myList5.push(...filteredValues);
             }
            const checkbox11 = $('#' + checkboxId);

            if (checkbox11.is(':checked')) {

                if (!myList5.includes(RecordId)) {
                    myList5.push(RecordId);
                }  
            } else {
                 var chkid = "";
                chkid =  RecordId.toString();
                const index = myList5.indexOf(chkid);
                if (index > -1) {
                    myList5.splice(index, 1);
                }
            }
            $('#<%=hdnlistingstatus.ClientID%>').val(myList5.join(','));
            document.getElementById("<%=hdnlistingstatus.ClientID%>").value = ($('#<%=hdnlistingstatus.ClientID%>').val());

        }
          searchdata();

    }
       const myList3 = [];
       const myList4 = [];
      function Binddetails_Proptysubtype(RecordId,Recordfor, checkboxId)
      {
        
             
          if (Recordfor == "1") {
              if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdnresidentialval.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList3.length = 0;
                myList3.push(...filteredValues);
             }
            
              const checkbox = $('#' + checkboxId);
             
              if (checkbox.is(':checked')) {
                   
                  if (!myList3.includes(RecordId)) {
                      myList3.push(RecordId);
                  } else {

                  }
              } else {
                   var chkid = "";
                chkid =  RecordId.toString();
                  const index = myList3.indexOf(chkid);
                  if (index > -1) {
                      myList3.splice(index, 1);
                  }
              }
              $('#<%=hdnresidentialval.ClientID%>').val(myList3.join(','));
              document.getElementById("<%=hdnresidentialval.ClientID%>").value = ($('#<%=hdnresidentialval.ClientID%>').val());
               
          }
           if (Recordfor == "2") {
               if (document.getElementById("<%=hdnleadeditid.ClientID%>").value != ""&&document.getElementById("<%=hdnleadeditid.ClientID%>").value != "0") {
                const values = document.getElementById("<%=hdncommercialval.ClientID%>").value.split(',').map(value => value.trim());
                const filteredValues = values.filter(value => value !== '');
                myList4.length = 0;
                myList4.push(...filteredValues);
             }
              const checkbox = $('#' + checkboxId);
              if (checkbox.is(':checked')) {
                  if (!myList4.includes(RecordId)) {
                      myList4.push(RecordId);
                  } else {

                  }
              } else {
                  var chkid = "";
                chkid =  RecordId.toString();
                  const index = myList4.indexOf(chkid);
                  if (index > -1) {
                      myList4.splice(index, 1);
                  }
              }
              $('#<%=hdncommercialval.ClientID%>').val(myList4.join(','));
              document.getElementById("<%=hdncommercialval.ClientID%>").value = ($('#<%=hdncommercialval.ClientID%>').val());
         
           }
          searchdata();
       

            }
            function binddata()
            {try {
                    debugger
                    var idsemp = document.getElementById("<%=hdnlocation.ClientID%>").value.split(',');
                    $(document.getElementById("<%=ddllocation.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=ddllocation.ClientID%>")).change();
                }
                catch
                {}
                try {
                    debugger
                    var idsemp = document.getElementById("<%=hdnddLeadProduct.ClientID%>").value.split(',');
                    $(document.getElementById("<%=LeadProduct.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=LeadProduct.ClientID%>")).change();
                }
                catch
                {}
                var arr1;
                //var sts = "";
                if (document.getElementById("<%=hdisrenbuy.ClientID%>").value == "2")
                {
                    document.getElementById("lblrentbuy").innerHTML="Rent";
                    document.getElementById("pro_rent1").click();
                    //swiper = new Swiper(".pagination-fraction-swiper", { loop: !0, autoplay: { delay: 2500, disableOnInteraction: !1 }, pagination: { clickable: !0, el: ".swiper-pagination", type: "fraction" }, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" } });
if (document.getElementById("<%=hdnrentid.ClientID%>").value != "") {
                    arr1 = document.getElementById("<%=hdnrentid.ClientID%>").value.split(',');
                    if (arr1.length > 0) {
                        for (var i = 0; i < arr1.length; i++)
                        {
                            const checkbox = $('#rent' + arr1[i]);
                            checkbox.prop('checked', true);
                            //checkbox.checked=true;
                        }
                    }
                }
                }
                else if (document.getElementById("<%=hdisrenbuy.ClientID%>").value == "1")
                {
                    document.getElementById("lblrentbuy").innerHTML="Buy";
                    document.getElementById("pro_buy1").click();
                    //swiper = new Swiper(".pagination-fraction-swiper", { loop: !0, autoplay: { delay: 2500, disableOnInteraction: !1 }, pagination: { clickable: !0, el: ".swiper-pagination", type: "fraction" }, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" } });
     if (document.getElementById("<%=hdnbuyid.ClientID%>").value != "") {
                        arr1 = document.getElementById("<%=hdnbuyid.ClientID%>").value.split(',');
                        if (arr1.length > 0) {
                            for (var i = 0; i < arr1.length; i++)
                            {
                                const checkbox = $('#buy' + arr1[i]);
                                 checkbox.prop('checked', true);
                            }
                        }
                    }
                }
                
               
                if (document.getElementById("<%=hdnlistingbeds.ClientID%>").value != "") {
                    arr1 = document.getElementById("<%=hdnlistingbeds.ClientID%>").value.split(',');
                    if (arr1.length > 0) {
                        for (var i = 0; i < arr1.length; i++)
                        {
                            const checkbox = $('#beds' + arr1[i]);
                             checkbox.prop('checked', true);
                        }
                    }
                }
                 if (document.getElementById("<%=hdnlistingBaths.ClientID%>").value != "") {
                    arr1 = document.getElementById("<%=hdnlistingBaths.ClientID%>").value.split(',');
                    if (arr1.length > 0) {
                        for (var i = 0; i < arr1.length; i++)
                        {
                            const checkbox = $('#Baths' + arr1[i]);
                             checkbox.prop('checked', true);
                        }
                    }
                }
                if (document.getElementById("<%=hdisResidential.ClientID%>").value == "1") {
                    document.getElementById("lblResidential").innerHTML = "Residential";
                     document.getElementById("pro_residential1").click();
                    if (document.getElementById("<%=hdnresidentialval.ClientID%>").value != "") {
                        arr1 = document.getElementById("<%=hdnresidentialval.ClientID%>").value.split(',');
                        if (arr1.length > 0) {
                            for (var i = 0; i < arr1.length; i++) {
                                const checkbox = $('#residential' + arr1[i]);
                                checkbox.prop('checked', true);
                            }
                        }
                    }
                }
                if (document.getElementById("<%=hdisResidential.ClientID%>").value == "2") {
                    document.getElementById("lblResidential").innerHTML = "Commercial";
                     document.getElementById("pro_commercial1").click();
                    if (document.getElementById("<%=hdncommercialval.ClientID%>").value != "") {
                        arr1 = document.getElementById("<%=hdncommercialval.ClientID%>").value.split(',');
                        if (arr1.length > 0) {
                            for (var i = 0; i < arr1.length; i++) {
                                const checkbox = $('#commercial' + arr1[i]);
                                checkbox.prop('checked', true);
                            }
                        }
                    }
                }
                if (document.getElementById("<%=hdnlistingstatus.ClientID%>").value != "") {
                    arr1 = document.getElementById("<%=hdnlistingstatus.ClientID%>").value.split(',');
                    if (arr1.length > 0) {
                        for (var i = 0; i < arr1.length; i++)
                        {
                            const checkbox = $('#Furnishing' + arr1[i]);
                             checkbox.prop('checked', true);
                        }
                    }
                }
                try {
                    if (document.getElementById("<%=hdnminbudget.ClientID%>").value != "") {
                        bindminbudgetdetails();
                        document.getElementById("<%=ddlminBudget.ClientID%>").value = document.getElementById("<%=hdnminbudget.ClientID%>").value;
                        bindmaxbudget();
                        if (document.getElementById("<%=hdnmaxbudget.ClientID%>").value != "")
                        {
                            document.getElementById("<%=ddlmaxBudget.ClientID%>").value = document.getElementById("<%=hdnmaxbudget.ClientID%>").value;
                        }
                    }
                }
                catch
                {}
                try {
                    if (document.getElementById("<%=hdnminsize.ClientID%>").value != "") {
                        bindminsizedetails();
                        document.getElementById("<%=ddlminsize.ClientID%>").value = document.getElementById("<%=hdnminsize.ClientID%>").value;
                        bindmaxsize();
                        if (document.getElementById("<%=hdnmaxsize.ClientID%>").value != "")
                        {
                            document.getElementById("<%=ddlmaxsize.ClientID%>").value = document.getElementById("<%=hdnmaxsize.ClientID%>").value;
                        }
                    }
                }
                catch
                { }
                searchdata();
            }
            </script>
        <div style="display:none">
            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
        </div>
        <asp:HiddenField ID="hdnrentid" runat="server" Value="0" />
     <asp:HiddenField ID="hdnbuyid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnleadeditid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnbtnstatus" runat="server" Value="save" />
                
                <div class="offcanvas-body p-0 overflow-hidden">
                    <div data-simplebar style="height: calc(100vh - 57px);">
                        <div class="acitivity-timeline p-4">
                            <div class="row gy-4">
                            
                              
                                   
                                        <div class="lead_form_body">
                                            <div class="row">
                                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                                    <div class="d-flex align-items-center" style="text-align: left">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Salutation", "2")%><%--<asp:RequiredFieldValidator InitialValue="0" Display="Dynamic" CssClass="error" runat="server" ID="reqSalutation" SetFocusOnError="true" ValidationGroup="btnsave" ControlToValidate="Salutation" ErrorMessage="*" />--%></label>
                                                    </div>
                                                    <cc1:DropDownListChosen ID="Salutation" CssClass="form-control" runat="server" Width="250px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="" AllowSingleDeselect="true">
                                                        <asp:ListItem Value="1">Mr.</asp:ListItem>
                                                        <asp:ListItem Value="2">Ms.</asp:ListItem>
                                                        <asp:ListItem Value="3">Mrs.</asp:ListItem>
                                                        <asp:ListItem Value="4">Dr.</asp:ListItem>
                                                        <asp:ListItem Value="5">Prof.</asp:ListItem>
                                                        <asp:ListItem Value="6">Mx.</asp:ListItem>
                                                    </cc1:DropDownListChosen>


                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Project ID", "2")%><asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ControlToValidate="fName" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                                    </div>
                                                    <asp:TextBox ID="fName" runat="server" class="form-control" placeholder="Project ID" ></asp:TextBox>
                                                    <%--<input type="text" class="form-control" placeholder="First Name" id="fName" runat="server">--%>
                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Project Name", "2")%>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" SetFocusOnError="true" ControlToValidate="lName" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%></label>
                                                    </div>
                                                     <asp:TextBox ID="lName" runat="server" class="form-control"  placeholder="Project Name"></asp:TextBox>
                                                    
                                                </div>
                                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkprojectWeightage" runat="server"  />  Do You Want Same Weightage on Milestone   
                                        </label>
                                        </div>
                                    </div>
                                </div>
                                                 <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Owner", "2")%>
                                                        </label>
                                                    </div>
                                                    <asp:DropDownList ID="LeadProduct" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Products ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                                </div>
                                                 <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Team", "2")%>
                                                        </label>
                                                    </div>
                                                    <asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Company", "2")%><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ControlToValidate="cName" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%></label>
                                                    </div>
                                                     <asp:TextBox ID="cName" runat="server" class="form-control"  placeholder="Company"></asp:TextBox>
                                                    
                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <label class="form-label"><%=fns.getltext("Industry", "2")%><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" SetFocusOnError="true" ControlToValidate="Industry" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%></label>
                                                    <cc1:DropDownListChosen ID="Industry" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Industry ..." AllowSingleDeselect="true" onchange="AddNewIndustry()">
                                                    </cc1:DropDownListChosen>

                                                    <div id="dvnewindustry" style="display: none">
                                                        <asp:HiddenField ID="hdnnewindustry" runat="server" Value="0" />
                                                         <asp:TextBox ID="txtnewindustry" runat="server" class="form-control"  placeholder="Industry Name"></asp:TextBox>
                                                        
                                                    </div>
                                                </div>
                                                <div class="col-xxl-6 col-lg-6">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Designation", "2")%></label>
                                                    </div>
                                                    <asp:TextBox ID="title" runat="server" class="form-control"  placeholder="Designation"></asp:TextBox>
                                                   
                                                </div>
                                                <div class="col-xxl-6 col-lg-6">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Phone", "2")%><asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" ControlToValidate="PhoneNo" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator></label>
                                                    </div>
                                                    <asp:TextBox ID="PhoneNo" runat="server" class="form-control"  placeholder="Phone No" onkeypress="return isnumericonly(event,this.id);"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" placeholder="Phone No" id="" runat="server" onkeypress="return isnumericonly(event,this.id);">--%>
                                                </div>
                                                <div class="col-xxl-6 col-lg-6">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Emaill", "2")%>   &nbsp;&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" SetFocusOnError="true" ControlToValidate="EmailId" ValidationGroup="btnsave"
                                                                ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                                Display="Dynamic" ErrorMessage="* Invalid email address" /></label>
                                                    </div>
                                                    <asp:TextBox ID="EmailId" runat="server" class="form-control"  placeholder="Email" ></asp:TextBox>
                                                    
                                                </div>
                                                <div class="col-xxl-6 col-lg-6">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Website", "2")%></label>

                                                    </div>
                                                    <asp:TextBox ID="website" runat="server" class="form-control"  placeholder="Website" ></asp:TextBox>
                                               <%--     <input type="text" class="form-control" placeholder="Website" id="" runat="server">--%>
                                                </div>
                                                 <div class="col-xxl-6 col-lg-6" style="display:none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Telegram Chat Id", "2")%> </label>
                                                    </div>
                                                    <asp:TextBox ID="boatid" runat="server" class="form-control"  placeholder="Telegram Chat Id" ></asp:TextBox>
                                                    
                                                </div>
                                                
                                               
                                               
                                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                                    
                                                    <ul class="property_fillters property_fillters_add">
                                        <li>
                                              <div class="search_box_l form-group" style="">
                                                      <asp:DropDownList ID="ddllocation" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Location" AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                               
                                                <i class="ri-map-pin-2-fill search-icon"></i>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                            <button type="button" class="btn rounded-pill btn-outline-primary"
                                                data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                  <asp:Label ID="lblrentbuy" runat="server" Text="Rent"></asp:Label>  
                                                
                                                
                                                <i class="ri-arrow-down-s-fill"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-lg p-2">
                                                 <asp:HiddenField ID="hdisrenbuy" runat="server" Value="2" />
                                                <!-- Nav tabs -->
                                                <ul class="nav nav-pills nav-justified mb-3" role="tablist">
                                                    <li class="nav-item mb-1">
                                                        <a class="nav-link active" id="pro_rent1" data-bs-toggle="tab" href="#pro_rent" role="tab" onclick="changetxtrentbuy('Rent','2')">Rent</a>
                                                    </li>
                                                    <li class="nav-item mb-1">
                                                        <a class="nav-link" id="pro_buy1" data-bs-toggle="tab" href="#pro_buy" role="tab" onclick="changetxtrentbuy('Buy','1')">Buy</a>
                                                    </li>
                                                </ul>
                                                <!-- Tab panes -->

                                                 

                                                <div class="tab-content text-muted">
                                                    <div class="tab-pane active" id="pro_rent" role="tabpanel">
                                                              <asp:Label ID="lblrent" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnrent" runat="server" Value="0" />
                                                     
                                                    </div>
                                                    <div class="tab-pane" id="pro_buy" role="tabpanel">
                                                          <asp:Label ID="lblbuy" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnbuy" runat="server" Value="0" />
                                                     
                                                    </div>  
                                                </div>

                                                 


                                               <%-- <div class="text-right">
                                                <a href="" class="btn-link">Clear</a>
                                                </div>--%>
                                            </div>
                                        </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                            <button type="button" class="btn rounded-pill btn-outline-primary"
                                                data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                Bedrooms <i class="ri-arrow-down-s-fill"></i>
                                            </button>

                                          

                                            <div class="dropdown-menu dropdown-menu-lg p-2">
                                                <h6 class="mb-2">No. of Bedrooms</h6>
                                                <div class="hstack gap-2 flex-wrap beds_btn mb-1">


                                                      <asp:Label ID="listingbeds" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnlistingbeds" runat="server" Value="0" />

                                                   
                                                </div>
                                                <%--<div class="text-right">
                                                <a href="" class="btn-link">Clear</a>
                                                </div>--%>
                                            </div>



                                        </div>
                                        </li>
                                        <li style="display:none">
                                            <div class="btn-group">
                                            <button type="button" class="btn rounded-pill btn-outline-primary"
                                                data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                Bathrooms <i class="ri-arrow-down-s-fill"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-lg p-2">
                                                <h6 class="mb-2">No. of Bathrooms</h6>
                                                <div class="hstack gap-2 flex-wrap beds_btn mb-1">

                                                      <asp:Label ID="listingBaths" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnlistingBaths" runat="server" Value="0" />
                                                   
                                                </div>
                                               <%-- <div class="text-right">
                                                <a href="" class="btn-link">Clear</a>
                                                </div>--%>
                                            </div>
                                        </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                            <button type="button" class="btn rounded-pill btn-outline-primary"
                                                data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                 
                                            <asp:Label ID="lblResidential"  runat="server" Text="Residential"></asp:Label>   
                                                <asp:HiddenField ID="hdisResidential" runat="server" />
                                                <i class="ri-arrow-down-s-fill"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-lg p-2">
                                                <!-- Nav tabs -->
                                                <ul class="nav nav-pills nav-justified mb-3" role="tablist">
                                                    <li class="nav-item mb-1">
                                                        <a class="nav-link active" id="pro_residential1" data-bs-toggle="tab" href="#pro_residential" role="tab"  onclick="changetxtResidentialCommercial('Residential','1')">Residential</a>
                                                    </li>
                                                    <li class="nav-item mb-1">
                                                        <a class="nav-link" id="pro_commercial1" data-bs-toggle="tab" href="#pro_commercial" role="tab"  onclick="changetxtResidentialCommercial('Commercial','2')">Commercial</a>
                                                    </li>
                                                </ul>
                                                <!-- Tab panes -->
                                                <div class="tab-content text-muted">
                                                    <div class="tab-pane active" id="pro_residential" role="tabpanel">

                                                        
                                                          <asp:Label ID="lblresidentialval" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnresidentialval" runat="server" Value="0" />

                                                       
                                                    </div>
                                                    <div class="tab-pane" id="pro_commercial" role="tabpanel">

                                                          <asp:Label ID="lblcommercialval" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdncommercialval" runat="server" Value="0" />


                                                     
                                                    </div>  
                                                </div>
                                               <%-- <div class="text-right">
                                                <a href="" class="btn-link">Clear</a>
                                                </div>--%>
                                            </div>
                                        </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                                <button type="button" class="btn rounded-pill btn-outline-primary"
                                                    data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                    Budget <i class="ri-arrow-down-s-fill"></i>
                                                </button>
                                                <div class="dropdown-menu dropdown-menu-lg p-2">
                                                    <h6 class="mb-2">Budget (AED)</h6>
<div class="p-2">
    <div class="range-slider" style="display:none">
        <input type="text" class="js-range-slider" value="" />
    </div>
    <div class="extra-controls form-inline">
        <div class="form-group row">
            <div class="col-xs-6">
                <div class="d-flex align-items-center">
                    <label class="form-label flex-grow-1 mb-0">Min Budget</label>
                </div>
               <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>

                                                 <asp:AsyncPostBackTrigger ControlID="btnbind" EventName="Click" />

                                            </Triggers>
                                            <ContentTemplate>--%>
                 <asp:DropDownList ID="ddlminBudget" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Min Budget" AllowSingleDeselect="true" onchange="bindmaxbudget(this.id)">
                                                    </asp:DropDownList>
                                                <%--</ContentTemplate>
                     </asp:UpdatePanel>--%>
                <%--onchange="bindmaxbudget(this.id)"--%>
            </div>
            <div class="col-xs-6">
                <div class="d-flex align-items-center">
                    <label class="form-label flex-grow-1 mb-0">Max Budget</label>
                </div>
               <%--  <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <Triggers>

                                                <asp:AsyncPostBackTrigger ControlID="ButtonleadB" EventName="Click" />
                                                 <asp:AsyncPostBackTrigger ControlID="btnbind" EventName="Click" />

                                            </Triggers>
                                            <ContentTemplate>--%>
                <asp:DropDownList ID="ddlmaxBudget" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Max Budget" AllowSingleDeselect="true" onchange="searchdata();">
                                                    </asp:DropDownList>
                                                <%--</ContentTemplate>
                     </asp:UpdatePanel>--%>
               <%-- <input type="text" class="js-input-to form-control" style="border-radius:4px;" value="0" id="maxBudget" />--%>
            </div>
        </div>
    </div>
</div>


                                                   
                                                    <%--<div class="text-right">
                                                    <a href="" class="btn-link">Clear</a>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                                <button type="button" class="btn rounded-pill btn-outline-primary"
                                                    data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                    Size <i class="ri-arrow-down-s-fill"></i>
                                                </button>
                                                <div class="dropdown-menu dropdown-menu-lg p-2">
                                                    <h6 class="mb-2">Size (Sq.ft)</h6>
<div class="p-2">
    <div class="range-slider" style="display:none">
        <input type="text" class="js-range-slider" value="" />
    </div>
    <div class="extra-controls form-inline">
        <div class="form-group row">
            <div class="col-xs-6">
                <div class="d-flex align-items-center">
                    <label class="form-label flex-grow-1 mb-0">Min Size</label>
                </div>
                 <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <Triggers>

                                                 <asp:AsyncPostBackTrigger ControlID="btnbind" EventName="Click" />

                                            </Triggers>
                                            <ContentTemplate>--%>
                 <asp:DropDownList ID="ddlminsize" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Min Size" AllowSingleDeselect="true" onchange="bindmaxsize(this.id)">
                                                    </asp:DropDownList>
                                                <%--</ContentTemplate>
                     </asp:UpdatePanel>--%>
                <%--<input type="text" class="js-input-from form-control" style="border-radius:4px;" value="0" id="minsize" />--%>
            </div>
            <div class="col-xs-6">
                <div class="d-flex align-items-center">
                    <label class="form-label flex-grow-1 mb-0">Max Size</label>
                </div>
               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <Triggers>

                                                <asp:AsyncPostBackTrigger ControlID="ButtonleadS" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnbind" EventName="Click" />


                                            </Triggers>
                                            <ContentTemplate>--%>
                <asp:DropDownList ID="ddlmaxsize" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Max Size" AllowSingleDeselect="true" onchange="searchdata();">
                                                    </asp:DropDownList>
                                               <%-- </ContentTemplate>
                     </asp:UpdatePanel>--%>
                <%--<input type="text" class="js-input-to form-control" style="border-radius:4px;" value="0" id="maxize" />--%>
            </div>
        </div>
    </div>
</div>
                                                    
                                                   <%-- <div class="text-right">
                                                    <a href="" class="btn-link">Clear</a>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="btn-group">
                                            <button type="button" class="btn rounded-pill btn-outline-primary"
                                                data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">
                                                Status <i class="ri-arrow-down-s-fill"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-lg p-2">
                                                <h6 class="mb-2">Furnishing Status</h6>
                                                <div class="hstack gap-2 flex-wrap beds_btn mb-1">

                                                         <asp:Label ID="listingstatus" runat="server"></asp:Label>
                                                   <asp:HiddenField ID="hdnlistingstatus" runat="server" Value="0" />
                                                  
                                                </div>
                                                   
                                               <%-- <div class="text-right">
                                                <a href="" class="btn-link">Clear</a>
                                                </div>--%>
                                            </div>
                                        </div>
                                        </li>
                                       
                                    </ul>
                                                </div>
                                                <div class="col-xxl-12 col-lg-12" id="dvselecteddata" <%=str_selected %>>
                                                    <%=str_selecteddeatil %>
                                                </div>
                                               <div class="col-xxl-12 col-lg-12">
                                                    


                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Requirements", "2")%></label>

                                                    </div>
                                                    <textarea class="form-control" placeholder="Requirements" id="description" runat="server"></textarea>

                                                </div>
                                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Lead Status", "2")%>
                                                            <asp:RequiredFieldValidator CssClass="error" runat="server" SetFocusOnError="true" ID="RequiredFieldValidator7" ValidationGroup="btnsave" ControlToValidate="leadStatus" ErrorMessage="*" />
                                                        </label>

                                                    </div>

                                                    <cc1:DropDownListChosen ID="leadStatus" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="bindprobality();">
                                                    </cc1:DropDownListChosen>
                                                </div>
                                                 <div class="col-xxl-12 col-lg-12" id="dvprobablity" <%=strviewpropbablity %>>
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("Probability (%)", "2")%><asp:RequiredFieldValidator ID="RequiredFieldValidator6" SetFocusOnError="true" ControlToValidate="leadProbability" ValidationGroup="btnsave1"
                                                    runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                        </div>

                                        <input type="text" class="form-control" placeholder="Probability (%)" id="leadProbability" runat="server">
                                                     <asp:HiddenField ID="hdnleadProbability" runat="server" />
                                    </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <label class="form-label"><%=fns.getltext("Lead Source", "2")%></label>
                                                    <cc1:DropDownListChosen ID="LeadSource" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Lead Source ..." AllowSingleDeselect="true" onchange="AddNewSource()">
                                                    </cc1:DropDownListChosen>
                                                    <div id="dvnewsource" style="display: none">
                                                        <asp:HiddenField ID="hdnnewsource" runat="server" Value="0" />
                                                        <input type="text" id="txtnewsource" class="form-control" placeholder="Source Name" runat="server">
                                                    </div>
                                                </div>
                                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Owner", "2")%><asp:RequiredFieldValidator CssClass="error" runat="server" SetFocusOnError="true" ID="RequiredFieldValidator8" ValidationGroup="btnsave" ControlToValidate="Owner" ErrorMessage="*" />
                                                        </label>
                                                    </div>
                                                    <cc1:DropDownListChosen ID="Owner" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                    </cc1:DropDownListChosen>

                                                </div>

                                            </div>
                                        
                                            <div class="row">

                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Address", "2")%> </label>
                                                    </div>
                                                    <input type="text" class="form-control" placeholder="Office No./Building/Floor" id="AddressDetails" runat="server">
                                                </div>
                                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Street", "2")%>
                                                        </label>
                                                    </div>
                                                    <input type="text" class="form-control" placeholder="Street" id="StreetName" runat="server">
                                                </div>
                                               
                                                <div class="col-xxl-5 col-lg-5" style="display:none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Zip/Postal Code", "2")%>
                                                        </label>
                                                    </div>
                                                    <input type="text" class="form-control" placeholder="Zip/Postal Code" id="PostalCode" runat="server">
                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("Country", "2")%>
                                                        </label>
                                                    </div>
                                                    <cc1:DropDownListChosen ID="CountryId" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Country Name ..." AllowSingleDeselect="true" onchange="AddNewstate();">
                                                    </cc1:DropDownListChosen>
                                                    <div id="dvnewcountry" style="display: none">
                                                        <asp:HiddenField ID="hdnnewcountry" runat="server" Value="0" />
                                                        <input type="text" class="form-control" placeholder="Country" id="CountryName" runat="server">
                                                    </div>

                                                </div>
                                                <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("State/Province", "2")%>
                                                        </label>
                                                    </div>
                                                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <Triggers>

                                                 <asp:AsyncPostBackTrigger ControlID="btncountry" EventName="Click" />

                                            </Triggers>
                                           <ContentTemplate>
                                                    <cc1:DropDownListChosen ID="StateId" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="State Name ..." AllowSingleDeselect="true" onchange="AddNewcity();">
                                                    </cc1:DropDownListChosen>
                                                   
                                                <div style="display: none">
                                                   <asp:Button ID="btncountry" runat="server" Text="Button" OnClick="btncountry_Click" />
                                                        <asp:HiddenField ID="hdnnewstate" runat="server" Value="0" />
                                                        
                                            </div>
                                               <asp:Panel ID="pnlstate" runat="server" Visible="false">
                                                        <input type="text" class="form-control" placeholder="State/Province" id="StateName" runat="server">
                                                    </asp:Panel>
                                                     </ContentTemplate>
                     </asp:UpdatePanel>
                                                     
                                                </div>
                                               
                                                 <div class="col-xxl-12 col-lg-12">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1">
                                                            <%=fns.getltext("City", "2")%>
                                                        </label>
                                                    </div>
                                                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>

                                                 <asp:AsyncPostBackTrigger ControlID="btnstateid" EventName="Click" />

                                            </Triggers>
                                           <ContentTemplate>
                                                     <cc1:DropDownListChosen ID="CityId" CssClass="form-control" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="City Name ..." AllowSingleDeselect="true" onchange="showcity();">
                                                    </cc1:DropDownListChosen>
                                                        
                                               <div style="display: none">
                                                   <asp:HiddenField ID="hdnnewcity" runat="server" Value="0" />                                                       
                                            <asp:Button ID="btnstateid" runat="server" Text="Button" OnClick="btnstate_Click" />
                                                   <asp:Button ID="btncity" runat="server" Text="Button" OnClick="btncity_Click" />
                                      </div>
                                               
                                               <asp:Panel ID="pnlcity" runat="server" Visible="false">
                                                   <div id="dvcity" <%=strdvcity%>>
                                                         <input type="text" class="form-control" placeholder="City" id="CityName" runat="server">
                                                       </div>
                                                    </asp:Panel> 
                    </ContentTemplate>
                     </asp:UpdatePanel>
                                                     
                                                </div>
                                                
                                            </div>
                                            <%if (isleadnotes==1)
                                                {%>
                                         <div class="row">
<div class="col-xxl-12 col-lg-12">
                                                    


                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Lead Notes", "2")%></label>

                                                    </div>
                                                    <textarea class="form-control" placeholder="Notes" id="txtlnotes" runat="server"></textarea>

                                                </div>
                                         </div>
                                            <%} %>
                                            <div class="row">
                                                <div class="col-xxl-6 col-lg-6" style="display: none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("No. of Employees", "2")%></label>
                                                    </div>
                                                    <input type="text" class="form-control" placeholder="No. of Employees" id="NoOfemp" runat="server">
                                                </div>
                                                <div class="col-xxl-6 col-lg-6" style="display: none">
                                                    <div class="d-flex align-items-center">
                                                        <label class="form-label flex-grow-1"><%=fns.getltext("Annual Revenue", "2")%></label>
                                                    </div>
                                                    <input type="text" class="form-control" placeholder="Annual Revenue" id="AnnualRevenue" runat="server">
                                                </div>


                                            </div>
                                        </div>
                                    
                            </div>
                        </div>
                    </div>
                </div>
                            <div class="offcanvas-foorter border p-2 text-right" style="text-align: center">
                   <a href="#" class="btn btn-danger" aria-label="Close" onclick="showdivimglead5();">Close</a>
                    <a href="#" class="btn btn-danger" style="display:none" id="btncloselead5" data-bs-dismiss="offcanvas" aria-label="Close" onclick="showdivimglead5();">Close</a>
                     <div id="pnlimg" style="display: none;">
                        <img src="/images/loader.gif" runat="server" id="img1" />
                    </div>
                    <div class="btn-group">
                       <%-- <a href="#" onclick="SvaeData('save');" class="btn btn-success">Save</a>--%>
                        <div class="btn-group" id="divsub" style="display: block;">
                        <asp:Button ID="btnsave" OnClientClick=" return savedata(1);" OnClick="btnsave_Click"  CssClass="btn btn-success" runat="server" Text="Save" ValidationGroup="btnsave" />
                         <asp:Button ID="btnsavemore" OnClientClick="return savedata(2);" OnClick="btnsavemore_Click"  CssClass="btn btn-secondary" runat="server" Text="Save & More" ValidationGroup="btnsave" />
</div>
                       
                    </div>
                    <!-- /btn-group -->
                                <asp:HiddenField ID="hdnsavetype" runat="server" />
                </div>
               <div style="display: none">

                   <asp:Button ID="ButtonleadB" runat="server" Text="" OnClick="ButtonleadB_Click" />
                   <asp:Button ID="ButtonleadS" runat="server" Text="" OnClick="ButtonleadS_Click" />
                   <asp:Button ID="btnbind" runat="server" Text=""  OnClick="btnbind_Click"/>
                   <asp:HiddenField ID="hdnbindtype" runat="server" Value="2" />
                    <asp:HiddenField ID="hdnddlminB" runat="server" Value="" />
                   <asp:HiddenField ID="hdnddlmaxB" runat="server" Value="" />
                   <asp:HiddenField ID="hdnddlminS" runat="server" Value="" />
                   <asp:HiddenField ID="hdnddlmaxS" runat="server" Value="" />
                   </div>
        <script>
function AddNewIndustry()
{
debugger
if(document.getElementById('<%=Industry.ClientID%>').value=="-1")
    {
     document.getElementById('dvnewindustry').style.display='';

    }
    else
    { 
    document.getElementById('dvnewindustry').style.display='none';
    
    }
}
function showcity()
{
debugger
if(document.getElementById('<%=CityId.ClientID%>').value=="-1")
    {
     document.getElementById('dvcity').style.display='';
document.getElementById('<%=btncity.ClientID%>').click();
    }
    else
    { 
    document.getElementById('dvcity').style.display='none';
    
    }
}
function AddNewstate()
{
    if(document.getElementById('<%=CountryId.ClientID%>').value=="-1")
    {
     document.getElementById('dvnewcountry').style.display='';
    }
    else
    { 
    document.getElementById('dvnewcountry').style.display='none';
    document.getElementById('<%=btncountry.ClientID%>').click();
    }
}
function AddNewcity()
{
debugger
    <%--if(document.getElementById('<%=StateId.ClientID%>').value=="-1")
    {
     document.getElementById('dvnewcity').style.display='';
    }
    else
    { 
     document.getElementById('dvnewcity').style.display='none';--%>
    document.getElementById('<%=btnstateid.ClientID%>').click();
   // }
}
//bindprobality();
function bindprobality() {
        debugger
        var ddl = document.getElementById('<%=leadStatus.ClientID%>').value;
        $.ajax({
            url: '<%=ResolveUrl("AddNewLeads.aspx/bindProbality") %>',
            data: "{ 'stageid': '" + ddl + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: OnSuccessProbality,
            error: function (response) {
                //alert(response.responseText);
                alert(response.responseText);
            },
            failure: function (response) {
                alert(response.responseText);
            }
        });

    }
    function OnSuccessProbality(response) {

        debugger

        var Itemdetails = $($.parseXML(response.d)).find("Table");
        var str = "";
        if (Itemdetails.find("id").text() != "") {
            document.getElementById('<%=leadProbability.ClientID%>').value = Itemdetails.find("LProbability").text();
            document.getElementById('<%=hdnleadProbability.ClientID%>').value = Itemdetails.find("LProbability").text();

        }

    }
        </script>
     <script type="text/javascript">
                                                 function showdivimglead5() {
                                                     if (Page_ClientValidate("button")) {
                                                         if (confirm('Do you want Leave this page ?')) {
                                                             window.parent.document.getElementById("btncloseleadclosenew").click();
                                                             return true;
                                                         }
                                                         else {
                                                             return false;
                                                         }
                                                     }
         }

           function clear() {
        debugger
                        $("[id*=hdnleadeditid]").val("0");
                        $("[id*=Salutation]").val("");
                        $("[id*=fName]").val("");
                        $("[id*=lName]").val("");
                        $("[id*=cName]").val("");
                        $("[id*=title]").val("");
                        $("[id*=website]").val("");
                        $("[id*=description]").val("");
                        $("[id*=leadStatus]").val("0");
                        $("[id*=Owner]").val("0");
                        $("[id*=PhoneNo]").val("");
                        $("[id*=EmailId]").val("");
                        $("[id*=AddressDetails]").val("");
                        $("[id*=StreetName]").val("");
                        $("[id*=CityName]").val("");
                        $("[id*=PostalCode]").val("");
                        $("[id*=StateName]").val("");
                        $("[id*=CountryName]").val("");
                        $("[id*=NoOfemp]").val("");
                        $("[id*=AnnualRevenue]").val("");
                        $("[id*=LeadSource]").val("0");
                        $("[id*=Industry]").val("0");
                        
                    }
            </script>
               <script type="text/javascript">
                    function isnumericonly(event,txtid) {
                debugger
                var textbox = document.getElementById(txtid);
                var charCode = (event.which) ? event.which : event.keyCode;
                // alert(charCode);
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    alert('Please enter only no value');
                    textbox.value = '';
                    return false;
                }
                else {
                    return true;
                }

                   }
                   function searchdata() { 


        var searchparm = "'hdisrenbuy': '" + document.getElementById('<%=hdisrenbuy.ClientID%>').value + "'";
        searchparm += ",'hdisResidential': '" + document.getElementById('<%=hdisResidential.ClientID%>').value + "'";
        searchparm += ",'isrentids': '" + document.getElementById('<%=hdnrentid.ClientID%>').value + "'";
        searchparm += ",'hdnbuyid': '" + document.getElementById('<%=hdnbuyid.ClientID%>').value + "'";
        searchparm += ",'hdnlistingbeds': '" + document.getElementById('<%=hdnlistingbeds.ClientID%>').value + "'"; 
        searchparm += ",'hdnlistingBaths': '" + document.getElementById('<%=hdnlistingBaths.ClientID%>').value + "'"; 
        searchparm += ",'hdnresidentialval': '" + document.getElementById('<%=hdnresidentialval.ClientID%>').value + "'"; 
        searchparm += ",'hdncommercialval': '" + document.getElementById('<%=hdncommercialval.ClientID%>').value + "'"; 
        searchparm += ",'hdnlistingstatus': '" + document.getElementById('<%=hdnlistingstatus.ClientID%>').value + "'";
        searchparm += ",'hdnlocation': '" + document.getElementById('<%=hdnlocation.ClientID%>').value + "'"; 
        searchparm += ",'minbudget': '" + document.getElementById('<%=ddlminBudget.ClientID%>').value + "'"; 
        searchparm += ",'maxbudget': '" + document.getElementById('<%=ddlmaxBudget.ClientID%>').value + "'"; 
        searchparm += ",'minsize': '" + document.getElementById('<%=ddlminsize.ClientID%>').value + "'"; 
        searchparm += ",'maxsize': '" + document.getElementById('<%=ddlmaxsize.ClientID%>').value + "'"; 
      


        //alert(searchparm);
              debugger;
                  $.ajax({
                      url: 'AddNewLeads.aspx/getsearchvalue',
                    data: "{ "+searchparm+"}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                      success: function (response) {
                          if (response.d != "") {
//alert(response.d);
                              //document.getElementById('dvselecteddata').style.display = ''
                              document.getElementById('dvselecteddata').innerHTML = response.d;
                          }
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
                   function savedata(svetype) {
                       debugger
                       document.getElementById('<%=hdnddlminS.ClientID%>').value = document.getElementById('<%=ddlminsize.ClientID%>').value;
                       document.getElementById('<%=hdnddlmaxS.ClientID%>').value = document.getElementById('<%=ddlmaxsize.ClientID%>').value;
                       document.getElementById('<%=hdnddlminB.ClientID%>').value = document.getElementById('<%=ddlminBudget.ClientID%>').value;
                       document.getElementById('<%=hdnddlmaxB.ClientID%>').value = document.getElementById('<%=ddlmaxBudget.ClientID%>').value;
                       //
                       //alert(document.getElementById('<%=ddlmaxsize.ClientID%>').value);
                       //return false;
                        document.getElementById('<%=hdnsavetype.ClientID%>').value = hdnsavetype;
                        <%--if ('<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2172'||'<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2201'|| '<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2203')
                        {
                         if (document.getElementById('<%=Industry.ClientID%>').value == "" || document.getElementById('<%=Industry.ClientID%>').value == "0"|| document.getElementById('<%=Industry.ClientID%>').value == "-1") {
                           alert('Please Select Industry?');
                           document.getElementById('<%=Industry.ClientID%>').focus();
                           return false;
                          }
                        }--%>
                   <%--if (document.getElementById('<%=Salutation.ClientID%>').value == "" || document.getElementById('<%=Salutation.ClientID%>').value == "0") {
                           alert('Please Select Salutation?');
                           document.getElementById('<%=Salutation.ClientID%>').focus();
                           return false;
                       }--%>
                      <%-- if (document.getElementById('<%=Owner.ClientID%>').value == "" || document.getElementById('<%=Owner.ClientID%>').value == "0") {
                           alert('Please Select Owner?');
                           document.getElementById('<%=Owner.ClientID%>').focus();
                           return false;
                       }--%>
                       if (Page_ClientValidate("btnsave")) {
                    
                           if (confirm('Are You Sure You want to Add This Entry!')) {
<%--'<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2172'||--%>
                        if ('<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2201'|| '<%=HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString()%>'=='2203')
                        {
                         if (document.getElementById('<%=Industry.ClientID%>').value == "" || document.getElementById('<%=Industry.ClientID%>').value == "0"|| document.getElementById('<%=Industry.ClientID%>').value == "-1") {
                           alert('Please select industry');
                           document.getElementById('<%=Industry.ClientID%>').focus();
                           return false;
                          }
                        }
                        
                               document.getElementById("pnlimg").style.display = "block";
                               document.getElementById("divsub").style.display = "none";
                            
                           }
                           else {
                               return false;
                           }
                       }
                   }
                        
                       
      
                   
            </script> 
           

    <asp:HiddenField ID="hdnddLeadProduct" runat="server" />
        <asp:HiddenField ID="hdnlocation" runat="server" />
        <asp:HiddenField ID="hdnminsize" runat="server" />
        <asp:HiddenField ID="hdnmaxsize" runat="server" />
        <asp:HiddenField ID="hdnminbudget" runat="server" />
        <asp:HiddenField ID="hdnmaxbudget" runat="server" />
        <script src="/crm/assets/js/select2.min.js"></script>
        <link href="/crm/assets/js/select2.min.css" rel="stylesheet" type="text/css">
        <script>
            //debugger
            var LeadProduct = $(document.getElementById("<%=LeadProduct.ClientID%>"));


            $(LeadProduct).select2({
                data: [
    <%=Multiproduct%>
                ],
                multiple: true,
                placeholder: "Search Products",
                width: "100%"
            });


            $(document.getElementById("<%=LeadProduct.ClientID%>")).change(function () {
                //debugger
                var ids = $(document.getElementById("<%=LeadProduct.ClientID%>")).val();
         document.getElementById("<%=hdnddLeadProduct.ClientID %>").value = ids;
         document.getElementById("<%=LeadProduct.ClientID%>").click();

            });


        </script>
        <script>
            //debugger
            var txtlocation = $(document.getElementById("<%=ddllocation.ClientID%>"));


            $(txtlocation).select2({
                data: [
    <%=mcatReason%>
                ],
                multiple: true,
                placeholder: "Search Location",
                width: "100%"
            });


            $(document.getElementById("<%=ddllocation.ClientID%>")).change(function () {
                //debugger
                var ids = $(document.getElementById("<%=ddllocation.ClientID%>")).val();
         document.getElementById("<%=hdnlocation.ClientID %>").value = ids;
        searchdata();

            });


        </script>

          <script src="/crm/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
        <script src="/crm/assets/libs/simplebar/simplebar.min.js"></script>
        <script src="/crm/assets/js/plugins.js"></script>
        <!-- prismjs plugin -->
        <script src="/crm/assets/libs/prismjs/prism.js"></script>
        <!-- apexcharts -->
        <script src="/crm/assets/libs/list.pagination.js/list.pagination.min.js"></script>
        <!-- Sweet Alerts js -->

        <script src="/crm/assets/libs/sweetalert2/sweetalert2.min.js"></script>
        <script src="/crm/assets/libs/list.js/list.min.js"></script>
        <script src="/crm/assets/js/pages/widgets.init.js"></script>
        <!-- dragula init js -->
        <!-- App js -->
        <script src="/crm/assets/js/jquery.dataTables.min.js"></script>
    </form>

</body>
</html>
