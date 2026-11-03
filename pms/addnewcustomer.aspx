<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="addnewcustomer.aspx.cs" Inherits="addnewcustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/ucontrols/DateSelector.ascx" TagName="uc1" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="img/favicon.ico" type="image/x-icon" /> 
    <link rel="stylesheet" type="text/css" href="~/css/cloud-admin.css">
    <link rel="stylesheet" type="text/css" href="~/css/themes/default.css">
    <link rel="stylesheet" type="text/css" href="~/css/responsive.css">
    <!-- STYLESHEETS -->
    <!--[if lt IE 9]><script src="js/flot/excanvas.min.js"></script><script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script><script src="http://css3-mediaqueries-js.googlecode.com/svn/trunk/css3-mediaqueries.js"></script><![endif]-->
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <!-- ANIMATE -->
    <link rel="stylesheet" type="text/css" href="/css/animatecss/animate.min.css" />
    <!-- DATE RANGE PICKER -->
    <link rel="stylesheet" type="text/css" href="/js/bootstrap-daterangepicker/daterangepicker-bs3.css" />
    <!-- TODO -->
    <link rel="stylesheet" type="text/css" href="/js/jquery-todo/css/styles.css" />
    <!-- FULL CALENDAR -->
    <link rel="stylesheet" type="text/css" href="/js/fullcalendar/fullcalendar.min.css" />
    <!-- GRITTER -->
    <link rel="stylesheet" type="text/css" href="/js/gritter/css/jquery.gritter.css" />
    <!-- FONTS -->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700' rel='stylesheet' type='text/css'>
    <link type="text/css" href="/assets/fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link type="text/css" href="/assets/fonts/themify-icons/themify-icons.css" rel="stylesheet">
    <!-- Themify Icons -->
    <link href="/DateSelector/cal/popcalendar.css" rel="Stylesheet" type="text/css" />
    <link type="text/css" href="/js/datatables/dataTables.bootstrap.css" rel="stylesheet">
    <!-- Bootstrap Support for Datatables -->
    <link type="text/css" href="/js/datatables/dataTables.themify.css" rel="stylesheet">
    <!-- FontAwesome Support for Datatables -->
    <script type="text/javascript" src="~/DateSelector/cal/popcalendar.js"></script>
    <style>
        .blockul {
            display: block;
        }

        .ulliActive {
            background-color: #450c3a !important;
            color: #fff !important;
        }

            .ulliActive i {
                color: #fff !important;
            }

            .ulliActive:hover {
                background-color: #450c3a !important;
                color: #fff !important;
            }

        .sidebar-menu > ul > li > ul.sub > li > a:hover {
            background-color: #450c3a !important;
            color: #fff !important;
        }

            .sidebar-menu > ul > li > ul.sub > li > a:hover i {
                color: #fff !important;
            }

        .contact-tabs {
            width: 100%;
        }

            .contact-tabs ul {
                list-style: none;
                margin: 10px 0;
                background-color: #fcfcfc;
                border-top: 1px solid #EEE;
                border-bottom: 1px solid #EEe;
                padding: 10px 0px;
                float: left;
                width: 100%;
            }

                .contact-tabs ul li {
                    list-style: none;
                    margin: 0px;
                    padding: 8px 10px;
                    border-radius: 2px;
                    float: left;
                }

                    .contact-tabs ul li a {
                        display: block;
                        font-size: 14px;
                    }

                    .contact-tabs ul li:hover {
                        background-color: #eee;
                    }

                    .contact-tabs ul li.active {
                        background-color: #72075d;
                        color: #fff;
                    }

                        .contact-tabs ul li.active a {
                            color: #fff;
                        }

        @media(max-width:500px) {
            .contact-tabs ul li {
                width: 50%;
            }
    </style>

    <script language="javascript">

        function isnumeric(evt) {
            var charcode = (evt.which) ? evt.which : event.KeyCode;
            if (charcode > 31 && (charcode < 46 || charcode > 57)) {
                alert("Please Enter Numeric Value !");
                return false;
            }
        }

    </script>


    <%--<script type="text/javascript" src="../Pagepopup/js/jquery.js"></script> 
      <script type="text/javascript" src="../Pagepopup/js/main.js"></script>  
  <script type="text/javascript" src="/js/jquery.min.js"></script>
      <link href="/popup/mypopup2.css" rel="stylesheet" type="text/css" />
  <script src="/popup/JScript.js" type="text/javascript"></script>--%>
    <script language="javascript">


</script>
    <style type="text/css">
        .modalPopup {
            background-color: #696969;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }


        .tbllist {
            background-color: #eaeff1;
            border: 1px solid #dcdadb;
            font-family: Arial;
            font-size: 12px;
            font-weight: normal;
            width: 100%;
        }

            .tbllist th {
                background-color: #2fa0a4;
                border: 1px solid #dcdadb;
                font-weight: bold;
                color: White;
            }

            .tbllist td {
                border: 1px solid #dcdadb;
            }

            .tbllist th a {
                color: #fff;
                display: block;
                padding: 1px;
            }

                .tbllist th a:hover {
                    background-color: #203e7a;
                    color: #fff;
                }

                .tbllist th a.curcol {
                    background: #203e7a url(../images/icons/top_white.gif) no-repeat right;
                }
    </style>















    <style>
        .divautocom {
            height: 100px;
            width: 150px !important;
            padding: 10px;
            float: left;
            overflow: auto;
            list-style: none;
            background-color: #fff;
            position: absolute !important;
        }

            .divautocom li {
                margin-bottom: 5px;
                width: 100% !important;
            }

        .selectitem {
            background-color: Yellow;
        }


        .radibuttonlistclass input {
            width: 15px;
            margin-right: 5px;
        }

        .radibuttonlistclass label {
            display: inline-block;
        }

        .contact-tabs {
            width: 100%;
        }

            .contact-tabs ul {
                list-style: none;
                margin: 25px 0 0 0;
                background-color: #fcfcfc;
                border-top: 1px solid #EEE;
                border-bottom: 1px solid #EEe;
                padding: 10px 16px;
                float: left;
                width: 100%;
            }

                .contact-tabs ul li {
                    list-style: none;
                    margin: 0px;
                    padding: 0px;
                    float: left;
                }

                    .contact-tabs ul li a {
                        border-radius: 3px;
                        padding: 6px 15px;
                        color: #1f9697;
                    }

                    .contact-tabs ul li:hover a {
                        background-color: #eee;
                    }

                    .contact-tabs ul li.active a {
                        background-color: #14bdbf;
                        color: #fff;
                    }

        span.multiselect-native-select {
            position: relative
        }

            span.multiselect-native-select select {
                border: 0 !important;
                clip: rect(0 0 0 0) !important;
                height: 1px !important;
                margin: -1px -1px -1px -3px !important;
                overflow: hidden !important;
                padding: 0 !important;
                position: absolute !important;
                width: 1px !important;
                left: 50%;
                top: 30px
            }

        .multiselect-container {
            position: absolute;
            list-style-type: none;
            margin: 0;
            padding: 0
        }

            .multiselect-container .input-group {
                margin: 5px
            }

            .multiselect-container > li {
                padding: 0;
                float: none !important;
            }

                .multiselect-container > li.active {
                    border: 0px;
                }

                .multiselect-container > li > a.multiselect-all label {
                    font-weight: 700
                }

                .multiselect-container > li.multiselect-group label {
                    margin: 0;
                    padding: 3px 20px 3px 20px;
                    height: 100%;
                    font-weight: 700
                }

                .multiselect-container > li.multiselect-group-clickable label {
                    cursor: pointer
                }

                .multiselect-container > li > a {
                    padding: 0;
                    /*background-color:#fff !important;
     color:#000000 !important;*/
                }

        .dropdown-menu li a {
            background-color: #fff !important;
            color: #000 !important;
        }

            .dropdown-menu li a:hover {
                background-color: #B4D4D7 !important;
            }

        .multiselect-container > li > a > label {
            margin: 0;
            height: 100%;
            cursor: pointer;
            font-weight: 400;
            padding: 3px 0 3px 30px
        }

            .multiselect-container > li > a > label.radio, .multiselect-container > li > a > label.checkbox {
                margin: 0
            }

            .multiselect-container > li > a > label > input[type=checkbox] {
                margin-bottom: 5px;
                margin-top: 10px;
            }

        .btn-group > .btn-group:nth-child(2) > .multiselect.btn {
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px
        }

        .form-inline .multiselect-container label.checkbox, .form-inline .multiselect-container label.radio {
            padding: 3px 20px 3px 40px
        }

        .form-inline .multiselect-container li a label.checkbox input[type=checkbox], .form-inline .multiselect-container li a label.radio input[type=radio] {
            margin-left: -20px;
            margin-right: 0
        }

        .multiselect.dropdown-toggle {
            min-width: 200px;
            text-align: left;
            position: relative;
            padding-left: 10px;
        }

            .multiselect.dropdown-toggle .caret {
                position: absolute;
                right: 10px;
                top: 13px;
            }

        .dd_chk_select {
            height: 25px !important;
        }
    </style>

    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.11/jquery-ui.min.js"></script>--%>

    <style>
        .ui-autocomplete {
            cursor: pointer;
            height: 120px;
            overflow-y: scroll;
        }

        ul.multiselect-container {
            margin: 0px;
            padding: 0px;
            border-radius: 0px;
        }

            ul.multiselect-container li {
                mmargin: 0px;
                padding: 0px;
            }

                ul.multiselect-container li a {
                    mmargin: 0px;
                    padding: 0px 10px;
                }
    </style>

</head>
<body style="background-color: #fff !important;">
    <form id="form1" runat="server">
        <link href="/assets/css/plugins/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />

        <asp:ToolkitScriptManager ID="ScriptManager1" runat="server"></asp:ToolkitScriptManager>

        <div style="background-color: #fff !important;">


            <table class="table table-striped table-bordered" style="display: none;">
                <tr>
                    <td width="200px"><%=fns.getltext("Party Type", "2")%></td>
                    <td>

                        <asp:RadioButtonList ID="ContactType" runat="server" RepeatDirection="Horizontal" CssClass="radibuttonlistclass"
                            AutoPostBack="true" OnSelectedIndexChanged="ContactType_change">

                            <asp:ListItem Value="1">Customer</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">Supplier</asp:ListItem>
                            <asp:ListItem Value="3" >Contractor</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>

                <table class="table table-striped table-bordered" style="<%=ispromax%>">
                <tr>
                    <td>
                        <a href="/jobcardpromax.aspx" target="_blank" class="btn btn-primary" style="float:right">Old Data</a>
                    </td>
                       </tr>
            </table>

            <div class="contact-tabs">
                <div class="nav-temptab-column">
                    <ul class="edit-center" style="margin-top:0">
                        <li class="active"><a data-toggle="tab" href="#home"><%=fns.getltext("Company Details", "2")%></a></li>
                        <li class=""><a data-toggle="tab" href="#menu1"><%=fns.getltext("Address Detail", "2")%></a></li>
                        <li id="creditlimit" runat="server" class=""><a data-toggle="tab" href="#menu4"><%=fns.getltext("Credit limit detail", "2")%></a></li>
                        <%--  <li class=""><a data-toggle="tab" href="#menu6"><%=fns.getltext("Price Fixing Detail", "2")%></a></li>--%>
                        <li class=""><a data-toggle="tab" href="#menu2"><%=fns.getltext("Other Persons detail", "2")%></a></li>
                        <li class=""><a data-toggle="tab" href="#menu3"><%=fns.getltext("Custom Field Detail", "2")%></a></li>

                        <%--  <li class=""><a data-toggle="tab" href="#menu4">Reporting Tags</a></li>--%>
                        <li class=""><a data-toggle="tab" href="#menu9"><%=fns.getltext("Bank Details", "2")%></a></li>

                        <li class=""><a data-toggle="tab" href="#menu5"><%=fns.getltext("Remarks", "2")%></a></li>
                    </ul>
                </div>
                <div class="nav-temptab-content">
                    <div class="tab-content">
                        <div id="home" class="tab-pane fade in active">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered datatables">



                                    <tr>
                                        <td>
                                            <%=fns.getltext("Under", "2")%>
                                              <asp:Label ID="lbltxtunder" runat="server" ForeColor="Red"   Text="*"></asp:Label>
                                        </td>
                                        <td>


                                            <cc1:DropDownListChosen ID="txtunder" runat="server"
                                                NoResultsText="No results match." Width="250px"
                                                DataPlaceHolder="Type Here..." AllowSingleDeselect="true">
                                            </cc1:DropDownListChosen>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="gi" InitialValue="0" Text="*" ControlToValidate="txtunder" runat="server" ErrorMessage="Please select Group Name !!!"></asp:RequiredFieldValidator>

                                            <asp:HiddenField ID="hdnempid" runat="server" />



                                        </td>

                                    </tr>

                                    <tr id="trcustomertype" runat="server" visible="false">
                                        <td>
                                            <%=fns.getltext("Price Category", "2")%>
                                        </td>
                                        <td>


                                            <cc1:DropDownListChosen ID="ddlcustomertype" runat="server"
                                                NoResultsText="No results match." Width="250px"
                                                DataPlaceHolder="Type Here..." AllowSingleDeselect="true">
                                            </cc1:DropDownListChosen>





                                        </td>

                                    </tr>




                                    <tr>
                                        <td width="200px"><%=fns.getltext("Primary Contact", "2")%></td>
                                        <td>
                                            <input id="txtsalutation" runat="server" class="" placeholder="Mr." type="text" style="width: 300px !important;" />

                                            <script type="text/javascript">




                                                var prm1 = Sys.WebForms.PageRequestManager.getInstance();
                                                if (prm1 != null) {
                                                    prm1.add_endRequest(function (sender, e) {
                                                        if (sender._postBackSettings.panelsToUpdate != null) {
                                                            BindControls2();
                                                        }
                                                    });
                                                };




                                                $(document).ready(function () {
                                                    BindControls2();

                                                });
                                                function BindControls2() {

                                                    var Countries = document.getElementById('<%=hdnSalutation.ClientID %>').value.split('##');


                                                       $('#<%=txtsalutation.ClientID %>').autocomplete({
                                                        source: Countries,
                                                        minLength: 0,
                                                        autoFocus: true,
                                                        scroll: true,
                                                        select: function (event, ui) {

                                                        }
                                                    }).focus(function () {
                                                        $(this).autocomplete("search", "");
                                                    });
                                                }

                                            </script>



                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("First Name", "2")%>
                                             <asp:Label ID="lbltxtfirstname" runat="server" ForeColor="Red"   Text="*"></asp:Label>
 
                                        </td>
                                        <td>
                                            <input id="txtfirstname" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"  ControlToValidate="txtfirstname" Text="*" Enabled="false" ErrorMessage="*" ValidationGroup="gi" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>

                                            </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Last Name", "2")%></td>
                                        <td>
                                            <input id="txtlastname" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td><%=codetext%></td>
                                        <td>

                                            <input id="txtvcode" runat="server" class=""   placeholder="" type="text" style="width: 300px !important; "  />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Company Name", "2")%>
                                             <asp:Label ID="lbltxtcompanyname" runat="server" ForeColor="Red"   Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <input id="txtcompanyname" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txtcompanyname" ErrorMessage="Please Enter Company Name"
                                                ValidationGroup="gi" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="<%=issborg%>">
                                        <td><%=fns.getltext("Drug License Number", "2")%></td>
                                        <td><input id="txtDLN" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" /></td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td><%=fns.getltext("Contact Display Name", "2")%></td>
                                        <td>
                                            <input id="txtcontactdisplayname" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Contact Email", "2")%></td>
                                        <td>

                                            <asp:TextBox ID="txtContactEmail" runat="server" Style="width: 300px !important;"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email Id"
                                                Display="Dynamic" SetFocusOnError="true" ForeColor="Red" ValidationGroup="gi"
                                                ControlToValidate="txtContactEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                            <%--  <input id="txtContactEmail"  runat="server"  class="" placeholder="" type="text" style="width:300px !important;" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Contact Phone", "2")%>
                                             <asp:Label ID="lbltxtmobile" runat="server" ForeColor="Red"   Text="*"></asp:Label>
                                        </td>
                                        <td>
                                            <input id="txtmobile" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                            <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="txtmobile" ErrorMessage="Please Enter Contact Phone"
                                                ValidationGroup="gi" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Work Phone", "2")%></td>
                                        <td>
                                            <input id="txtworkphonePhone" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Skype Name/Number", "2")%></td>
                                        <td>
                                            <input id="txtSkypeName" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Designation", "2")%></td>
                                        <td>
                                            <input id="txtDesignation" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Department", "2")%></td>
                                        <td>
                                            <input id="txtDepartment" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>

                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("Owner/Ceo Name", "2")%></td>
                                        <td>
                                            <input id="ceoname" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("Owner/Ceo Contact", "2")%></td>
                                        <td>
                                            <input id="ceoContact" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("Owner/Ceo Email", "2")%></td>
                                        <td>
                                            <input id="ceoEmail" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>

                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("Website", "2")%></td>
                                        <td>
                                            <input id="Website" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("PAN Number", "2")%></td>
                                        <td>
                                            <input id="panno" runat="server" class="" placeholder="" type="text" maxlength="10" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("Tan Number", "2")%></td>
                                        <td>
                                            <input id="Tanno" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr style="<%=turkeydispaly%>">
                                        <td><%=fns.getltext("CIN No", "2")%></td>
                                        <td>
                                            <input id="txtcinno" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                </table>


                                <asp:UpdatePanel ID="updatepanelvat" runat="server">
                                    <ContentTemplate>




                                        <table class="table table-striped table-bordered datatables">


                                            <tr>
                                                <td width="210px">
                                                    <asp:Label ID="lblVATTreatment" runat="server"></asp:Label></td>
                                                <td>

                                                    <asp:DropDownList ID="VATTreatment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="VATTreatment_change" Style="width: 300px !important">
                                                    </asp:DropDownList>


                                                </td>
                                            </tr>

                                            <tr id="VATTRNNumbertr" runat="server">
                                                <td>
                                                    <asp:Label ID="lblvattrnnumber" runat="server"></asp:Label>
 <asp:Label ID="lblTaxRegistrationNumber" runat="server" ForeColor="Red"   Text="*"></asp:Label>
 
                                                </td>
                                                <td>
                                                    <input id="TaxRegistrationNumber" runat="server" class="" maxlength="15" placeholder="" type="text" style="width: 300px !important" />

                                                    <asp:RegularExpressionValidator ID="RegExp1" runat="server"
                                                        ErrorMessage="VAT TRN Number length must be 15 characters"
                                                        ControlToValidate="TaxRegistrationNumber" ValidationGroup="gi"
                                                        ValidationExpression="^[a-zA-Z0-9\s]{15,15}$" />
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="false" ControlToValidate="TaxRegistrationNumber" ErrorMessage="*" ValidationGroup="gi" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Enabled="false" 
          ControlToValidate="TaxRegistrationNumber" ErrorMessage="*" ValidationGroup="gi" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegExpturkey" runat="server" Display="Dynamic"    
ErrorMessage="VAT TRN Number length must be 10 characters"
ControlToValidate="TaxRegistrationNumber" ValidationGroup="gi"   
ValidationExpression="^[a-zA-Z0-9\s]{10,10}$" />

                                                </td>
                                            </tr>
                                            <tr id="placeofsupplytr" runat="server" style="<%=isturk%>">
                                                <td>

                                                    <asp:Label ID="lbldeliveryplace" runat="server"></asp:Label>
                                                     <asp:Label ID="lblddlplaceofsupply" runat="server" ForeColor="Red"   Text="*"></asp:Label>


                                                </td>
                                                <td>
                                                    <cc1:DropDownListChosen ID="ddlplaceofsupply" runat="server"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Type Here..." AllowSingleDeselect="true">
                                                    </cc1:DropDownListChosen>
                                                    <%--    <asp:DropDownList ID="ddlplaceofsupply" runat="server">
                                                
                                                </asp:DropDownList>--%>
                                                    <asp:RequiredFieldValidator ID="rddd" runat="server" ControlToValidate="ddlplaceofsupply"
                                                        ErrorMessage="*" ValidationGroup="gi" Display="Dynamic" InitialValue="0">
                                                    </asp:RequiredFieldValidator>

                                                </td>
                                            </tr>

                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <table class="table table-striped table-bordered datatables">
                                    <tr>
                                        <td width="210px">
                                            <%=fns.getltext("City", "2")%> 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcity" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="210px">
                                            <%=fns.getltext("Region", "2")%> 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegion" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td><%=fns.getltext("Currency", "2")%></td>
                                        <td>
                                            <asp:DropDownList ID="txtCurrency" runat="server"></asp:DropDownList>

                                            <%--<input id="txtCurrency" runat="server" class="" placeholder="" type="text" style="width:300px !important" />--%>
                                            <asp:HiddenField ID="hdnCurrencyid" runat="server" />
                                            <asp:HiddenField ID="hdncurrid" runat="server" />

                                            <script>

                                                $(document).ready(function () {
                                                    BindControls();

                                                });
                                                function BindControls() {

                                                    var Countries = document.getElementById('<%=hdnCurrencyid.ClientID %>').value.split('##');


                     $('#<%=txtCurrency.ClientID %>').autocomplete({
                         source: Countries,
                         minLength: 3,

                         autoFocus: true,
                         scroll: true,
                         select: function (event, ui) {
                             //   alert(ui.item.value);
                             $.ajax({
                                 url: '/settings/ajax.asmx/getcursymboal',
                                 data: "{ 'prefixText': '" + ui.item.value + "'}",
                                 dataType: "json",
                                 type: "POST",
                                 contentType: "application/json; charset=utf-8",
                                 success: function (data) {


                                     var arr = data.d.split('###');
                                     //alert(data.d);

                                     document.getElementById('<%=hdncurrid.ClientID %>').value = arr[1];

                                                                },
                                                                error: function (response) {
                                                                    alert(response.responseText);
                                                                },
                                                                failure: function (response) {
                                                                    alert(response.responseText);
                                                                }
                                                            });
                                                        }
                                                    }).focus(function () {
                                                        $(this).autocomplete("search", "");
                                                    });
                                                }
                                            </script>


                                        </td>
                                    </tr>


                                    <tr style="<%=issalesperson%>">
                                        <td><%=fns.getltext("Sales Person", "2")%></td>
                                        <td>
                                            <cc1:DropDownListChosen ID="ddlemployee" runat="server"
                                                NoResultsText="No results match."
                                                DataPlaceHolder="Type Here..." AllowSingleDeselect="true">
                                            </cc1:DropDownListChosen>

                                        </td>
                                    </tr>


                                    <tr>
                                        <td><%=fns.getltext("Terms & Conditions(Internal)", "2")%></td>
                                        <td>
                                            <textarea id="PaymentTerms" class="" rows="3" runat="server" placeholder="" type="text" style="width: 300px !important" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td><%=fns.getltext("Terms & Conditions(Display)", "2")%></td>
                                        <td>
                                            <textarea id="PaymentTermsdisplay" class="" rows="3" runat="server" placeholder="" type="text" style="width: 300px !important" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Mode Of Payment", "2")%></td>
                                        <td>
                                            <input id="txtmodeofpayment" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Delivery Type", "2")%></td>
                                        <td>
                                            <input id="txtdeliverytype" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Payment terms", "2")%></td>
                                        <td>
                                            <input id="txtpterms" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Transport/Dispatch Through", "2")%></td>
                                        <td>
                                            <input id="Txttransport" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Freight Terms", "2")%></td>
                                        <td>
                                            <input id="txtfreightterms" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Spl Instructoin", "2")%></td>
                                        <td>
                                            <input id="txtSplInstructoin" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Sales Value Previous Year", "2")%></td>
                                        <td>
                                            <input id="txtsalevalueprvyear" runat="server" class="" placeholder="" type="text" style="width: 300px !important;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Facebook", "2")%></td>
                                        <td>
                                            <input id="Facebook" class="" runat="server" placeholder="" type="text" style="width: 300px !important" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%=fns.getltext("Twitter", "2")%></td>
                                        <td>
                                            <input id="Twitter" class="" runat="server" placeholder="" type="text" style="width: 300px !important" />
                                        </td>
                                    </tr>
                                    <tr style="<%=TCSdisplay%>">
                                        <td><%=fns.getltext("Do you Want to Enable TCS?", "2")%></td>
                                        <td>


                                            <asp:RadioButtonList ID="rdltcs" runat="server" Width="150px" RepeatDirection="Horizontal"
                                                CssClass="radibuttonlistclass">
                                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                <asp:ListItem Value="1">Yes </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td><%=fns.getltext("Tax deducted?", "2")%></td>
                                        <td>

                                            <asp:UpdatePanel ID="updatepanel3" runat="server">
                                                <ContentTemplate>

                                                    <asp:RadioButtonList ID="istds" runat="server" Width="150px"
                                                        RepeatDirection="Horizontal"
                                                        CssClass="radibuttonlistclass" AutoPostBack="true" OnSelectedIndexChanged="istds_change">
                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes </asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:Panel ID="pnltdssection" runat="server" Visible="false">
                                                        Section    <asp:Label ID="lbltxtsectionname" runat="server" ForeColor="Red"   Text="*"></asp:Label>

                                                        <cc1:DropDownListChosen ID="ddlsectionname" runat="server"
                                                            NoResultsText="No results match." Width="350px"
                                                            DataPlaceHolder="Type Here..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddlsectionname_change">
                                                        </cc1:DropDownListChosen>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ValidationGroup="giadds" Text="*"
                                                            InitialValue="0" ControlToValidate="ddlsectionname" SetFocusOnError="true"
                                                            runat="server" ErrorMessage="Please Select Section !!!"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtsectionname" runat="server" placeholder="Section name" Visible="false" Width="150px"> </asp:TextBox>


                                                        <asp:TextBox ID="testper" runat="server" placeholder="TDS%" Visible="false" Width="50px" onkeypress="return isnumeric(event)"></asp:TextBox>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator12" SetFocusOnError="true" ValidationGroup="giadds" Text="*"
                                                            ControlToValidate="testper"
                                                            runat="server" ErrorMessage="Please TDS% !!!" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator11" SetFocusOnError="true" ValidationGroup="giadds" Text="*"
                                                            ControlToValidate="txtsectionname"
                                                            runat="server" ErrorMessage="Please Enter Section Name !!!" Display="Dynamic"></asp:RequiredFieldValidator>








                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>





                        <div id="menu6" class="tab-pane fade">
                            <div class="table-responsive">

                                <asp:UpdatePanel ID="updatepanelprice" runat="server">
                                    <ContentTemplate>




                                        <table class="table table-striped table-bordered">
                                            <tr id="trcat" runat="server" style="display: none;">
                                                <td style="width: 250px;">
                                                    <asp:Label ID="lblcatname" runat="server"></asp:Label></td>
                                                <td>
                                                    <cc1:DropDownListChosen ID="ddlcat" runat="server"
                                                        NoResultsText="No results match." Width="350px"
                                                        DataPlaceHolder="Type Here..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddlcat_change">
                                                    </cc1:DropDownListChosen>
                                                    <%-- <asp:DropDownList ID="ddlcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcat_change"></asp:DropDownList>--%></td>
                                            </tr>
                                            <tr id="trsubcat" runat="server" style="display: none;">
                                                <td style="width: 250px;">
                                                    <asp:Label ID="lblsubcatname" runat="server"></asp:Label></td>
                                                <td>
                                                    <cc1:DropDownListChosen ID="ddlsubcat" runat="server"
                                                        NoResultsText="No results match." Width="350px"
                                                        DataPlaceHolder="Type Here..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddlsubcat_change">
                                                    </cc1:DropDownListChosen></td>
                                            </tr>
                                            <tr>

                                                <td colspan="2"></td>
                                            </tr>

                                        </table>
                                        <asp:GridView ID="GrdvDepartment" runat="server" ShowHeaderWhenEmpty="True"
                                            AutoGenerateColumns="False"
                                            DataKeyNames="id"
                                            CssClass="table table-striped table-bordered"
                                            OnRowDataBound="GrdvDepartment_RowDataBound" OnPreRender="grduser_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="50px">
                                                    <ItemTemplate><%# Container.DisplayIndex + 1 + ((GrdvDepartment.PageIndex) * (GrdvDepartment.PageSize))%> </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate><%=lblcatname.Text%></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#Eval("catname")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate><%=lblsubcatname.Text%></HeaderTemplate>
                                                    <ItemTemplate><%#Eval("subcatname")%> </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="250px">
                                                    <HeaderTemplate>Name</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#Eval("pname")%>
                                                        <asp:HiddenField ID="hdnitemid" runat="server" Value='<%#Eval("id")%>' />
                                                        <asp:HiddenField ID="hdninventroytype" runat="server" Value='<%#Eval("ptype")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Width="250px">
                                                    <HeaderTemplate>Price</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtprice" runat="server" onkeypress="return isnumeric(event)"></asp:TextBox>

                                                    </ItemTemplate>
                                                </asp:TemplateField>




                                            </Columns>
                                            <EmptyDataTemplate>No record available </EmptyDataTemplate>
                                        </asp:GridView>



                                    </ContentTemplate>

                                </asp:UpdatePanel>



                            </div>
                        </div>


















                        <div id="menu1" class="tab-pane fade">
                            <div class="table-responsive">
                                <table class="table">
                                    <tr>
                                        <td width="50%" style="padding: 0;">
                                            <table class="table table-striped table-bordered datatables">
                                                <tr>
                                                    <td colspan="2">
                                                            <asp:Label ID="Label17" runat="server"></asp:Label>
                                                       <%-- <%=fns.getltext("Billing Address", "2")%>--%>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200px"><%=fns.getltext("Attention", "2")%></td>
                                                    <td>
                                                        <input id="billAttention" runat="server" class="" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Address", "2")%></td>
                                                    <td>
                                                        <textarea id="billAddress1" runat="server" placeholder="" style="width: 100%"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <textarea id="billAddress2" runat="server" placeholder="" style="width: 100%"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("City", "2")%></td>
                                                    <td>
                                                        <input id="billCity" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("State", "2")%></td>
                                                    <td>
                                                        <input id="billState" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Zip Code", "2")%></td>
                                                    <td>
                                                        <input id="billZipCode" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Country", "2")%></td>
                                                    <td>
                                                        <input id="billCountry" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Phone", "2")%></td>
                                                    <td>
                                                        <input id="billPhone" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr id="trbillgst" runat="server">
                                                    <td><%=fns.getltext("GST NO", "2")%></td>
                                                    <td>
                                                        <input id="billgst" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Fax", "2")%></td>
                                                    <td>
                                                        <input id="billFax" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="50%" style="padding: 0;" runat="server" >
                                            <asp:Panel runat="server" ID="pnlShipping">
                                            <table class="table table-striped table-bordered datatables">
                                                <tr>
                                                    <td><%=fns.getltext("Shipping Address", "2")%></td>
                                                    <td align="right">
                                                        <a onclick="copydata()" href="javascript://"><i class="fa fa-download"></i><%=fns.getltext("Same as Billing address", "2")%></a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200px"><%=fns.getltext("Attention", "2")%></td>
                                                    <td>
                                                        <input id="ShipAttention" runat="server" class="" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Address", "2")%></td>
                                                    <td>
                                                        <textarea id="ShipAddress1" runat="server" placeholder="" style="width: 100%"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <textarea id="ShipAddress2" runat="server" placeholder="" style="width: 100%"></textarea>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("City", "2")%></td>
                                                    <td>
                                                        <input id="ShipCity" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("State", "2")%></td>
                                                    <td>
                                                        <input id="ShipState" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Zip Code", "2")%></td>
                                                    <td>
                                                        <input id="ShipZipCode" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Country", "2")%></td>
                                                    <td>
                                                        <input id="ShipCountry" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Phone", "2")%></td>
                                                    <td>
                                                        <input id="ShipPhone" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr id="trShipgst" runat="server">
                                                    <td><%=fns.getltext("GST NO", "2")%></td>
                                                    <td>
                                                        <input id="Shipgst" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><%=fns.getltext("Fax", "2")%></td>
                                                    <td>
                                                        <input id="ShipFax" class="" runat="server" placeholder="" type="text" style="width: 100%" />
                                                    </td>
                                                </tr>
                                            </table>

                                            </asp:Panel>



                                            <script>

                                                function copydata() {

                                                    document.getElementById("<%=ShipAttention.ClientID %>").value = document.getElementById("<%=billAttention.ClientID %>").value;
                                                        document.getElementById("<%=ShipAddress1.ClientID %>").value = document.getElementById("<%=billAddress1.ClientID %>").value;
                                                        document.getElementById("<%=ShipAddress2.ClientID %>").value = document.getElementById("<%=billAddress2.ClientID %>").value;
                                                        document.getElementById("<%=ShipCity.ClientID %>").value = document.getElementById("<%=billCity.ClientID %>").value;
                                                        document.getElementById("<%=ShipState.ClientID %>").value = document.getElementById("<%=billState.ClientID %>").value;
                                                        document.getElementById("<%=ShipZipCode.ClientID %>").value = document.getElementById("<%=billZipCode.ClientID %>").value;
                                                        document.getElementById("<%=ShipCountry.ClientID %>").value = document.getElementById("<%=billCountry.ClientID %>").value;
                                                        document.getElementById("<%=ShipPhone.ClientID %>").value = document.getElementById("<%=billPhone.ClientID %>").value;
                                                        document.getElementById("<%=ShipFax.ClientID %>").value = document.getElementById("<%=billFax.ClientID %>").value;


                                                }
                                            </script>





                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><strong><%=fns.getltext("Note", "2")%>:</strong> <%=fns.getltext("You can add and manage additional addresses from contact details section", "2")%>.</td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="menu2" class="tab-pane fade">



                            <asp:UpdatePanel ID="updatepaneladd" runat="server">

                                <ContentTemplate>


                                    <div style="overflow: auto; width: 100%;">

                                        <asp:HiddenField ID="hdnGridview1" runat="server" Value="1" />
                                        <asp:HiddenField ID="hdnSalutation" runat="server" Value="1" />

                                        <asp:GridView ID="Gridview1" runat="server" CssClass="table table-striped table-bordered table-hover" ShowHeader="true"
                                            AutoGenerateColumns="false" OnRowDataBound="rowdatabind" OnPreRender="grduser_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Salutation">
                                                    <ItemTemplate>

                                                        <asp:TextBox ID="Salutation" runat="server" placeholder="Salutation" Width="50px"></asp:TextBox>


                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="First Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="FirstName" runat="server" placeholder="First Name" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lastname" runat="server" placeholder="Last Name" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Email Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="EmailAddress" runat="server" placeholder="Email Address" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Work Phone">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="WorkPhone" runat="server" placeholder="Work Phone" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Mobile" runat="server" placeholder="Mobile" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Skype Name/Number">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="SkypeName" runat="server" placeholder="Skype Name/Number" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Designation" runat="server" placeholder="Designation" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Department" runat="server" placeholder="Department" Width="100px"></asp:TextBox>
                                                        </div>    
                 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlshowlinkbutton" runat="server" Style="display: none; width: 50px;">

                                                            <asp:LinkButton ID="btnadd" TabIndex="-1" runat="server" OnClick="btnadd_Click">Add</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="btddel" TabIndex="-1" runat="server" OnClick="btndelete_Click">Delete</asp:LinkButton>
                                                        </asp:Panel>

                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" />

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                        <asp:HiddenField ID="hdngridviewrow" runat="server" Value="1" />







                                        <script>




                                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                                            if (prm != null) {
                                                prm.add_endRequest(function (sender, e) {
                                                    if (sender._postBackSettings.panelsToUpdate != null) {
                                                        BindControls1();
                                                    }
                                                });
                                            };




                                            $(document).ready(function () {
                                                BindControls1();

                                            });
                                            function BindControls1() {

                                                var Countries = document.getElementById('<%=hdnSalutation.ClientID %>').value.split('##');


                for (var x = 0; x < document.getElementById('<%=hdngridviewrow.ClientID %>').value; x++) {




                                                    $('#ContentPlaceHolder1_Gridview1_Salutation_' + x).autocomplete({
                                                        source: Countries,
                                                        minLength: 0,
                                                        autoFocus: true,
                                                        scroll: true,
                                                        select: function (event, ui) {

                                                        }
                                                    }).focus(function () {
                                                        $(this).autocomplete("search", "");
                                                    });
                                                }
                                            }
                                        </script>




















                                    </div>

                                    <%--        <table  class="table table-striped table-bordered datatables">
     <tr>
                                            <td colspan="10" style="padding:5px;"><a href="/">+ Add Contact Person</a></td>
                                        </tr>
                                    </table> --%>



                                    <script>

                                        function shoqlink(divid) {

                                            document.getElementById(divid).style.display = "block";
                                        }

                                        function hidelink(divid) {

                                            document.getElementById(divid).style.display = "none";
                                        }

                                    </script>

                                </ContentTemplate>
                            </asp:UpdatePanel>




                        </div>


                        <div id="menu3" class="tab-pane fade">
                            <table class="table table-striped table-bordered datatables">



                                <asp:Panel ID="pnlcustomControl" runat="server">
                                </asp:Panel>




                                <%--    <tr>
                                            <td width="200px">test</td>
                                            <td>
                                                <input id="" class="" placeholder="" type="text" style="width:300px !important">
                                            </td>
                                        </tr>--%>
                                <tr>
                                    <td align="left" colspan="2">
                                        <span style="color: red; font-weight: bold;"><%=fns.getltext("Note", "2")%>:</span> <%=fns.getltext("You can add additional fields for your contacts and have these show up on your PDF by going to Settings  >> Preferences >> Contacts. You can also refine the address format of your contacts from there.", "2")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="menu4" class="tab-pane fade">
                            <table class="table table-striped table-bordered datatables">
                                <tr>
                                    <td><%=fns.getltext("Credit Limit Allowed", "2")%> </td>

                                    <td>
                                        <asp:TextBox ID="txtcreditlimit" runat="server" placeholder="Credit limit" onkeypress="return isnumeric(event)"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td><asp:Label runat="server" ID="lblcredit" Text="Credit Period Allowed"></asp:Label></td>

                                    <td>
                                        <asp:TextBox ID="txtcreditperiod" runat="server" placeholder="Credit Period" onkeypress="return isnumeric(event)"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div id="menu5" class="tab-pane fade">
                            <table class="table table-striped table-bordered datatables">
                                <tr>
                                    <td><%=fns.getltext("Remarks", "2")%> (<%=fns.getltext("For Internal Use", "2")%>)</td>
                                </tr>
                                <tr>
                                    <td>
                                        <textarea runat="server" id="remarks" rows="4" style="width: 60%;"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>


                        <div id="menu9" class="tab-pane fade">
                            <table class="table table-striped table-bordered datatables">
                                <tr>

                                    <td style="width: 200px;"><%=fns.getltext("Bank Name", "2")%> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbankname" runat="server" Style="width: 100%" placeholder="BANK NAME"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><%=fns.getltext("Account number", "2")%></td>
                                    <td>
                                        <asp:TextBox ID="txtaccountno" runat="server" Style="width: 100%" placeholder="ACCOUNT NUMBER"></asp:TextBox>


                                    </td>
                                </tr>
                                <tr>
                                    <td><%=fns.getltext("IFS CODE", "2")%> </td>
                                    <td>
                                        <asp:TextBox ID="txtifccode" runat="server" Style="width: 100%" placeholder="IFS CODE"></asp:TextBox>


                                    </td>
                                </tr>
                                <tr>
                                    <td><%=fns.getltext("Corporate ID", "2")%> </td>
                                    <td>
                                        <asp:TextBox ID="txtcorporateid" runat="server" Style="width: 100%" placeholder="CORPORATE ID"></asp:TextBox>


                                    </td>
                                </tr>



                                <tr>
                                    <td><%=fns.getltext("Branch", "2")%> </td>
                                    <td>
                                        <asp:TextBox ID="txtbranch" runat="server" Style="width: 100%" placeholder="BRANCHE"></asp:TextBox>


                                    </td>
                                </tr>

                                <tr>
                                    <td><%=fns.getltext("Branch Address", "2")%> </td>
                                    <td>
                                        <asp:TextBox ID="txtbranchaddress" runat="server" Style="width: 100%" placeholder="BRANCHE ADDRESS"></asp:TextBox>


                                    </td>
                                </tr>

                                <tr>
                                    <td><%=fns.getltext("Benificiary Mail ID", "2")%></td>
                                    <td>
                                        <asp:TextBox ID="txtbenificiaryemail" runat="server" Style="width: 100%" placeholder="BENIFICIARY MAIL ID"></asp:TextBox>


                                    </td>
                                </tr>

                            </table>
                        </div>

                    </div>


                    <table class="table table-striped table-bordered datatables">

                        <tr>

                            <td colspan="3">

                                <center>

                                    <div id="pnlimg" style="display: none;">
                                        <img src="/images/loading8.gif" runat="server" id="img1" />
                                    </div>
                                    <div id="divsub" style="display: block;">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="gi"></asp:ValidationSummary>


                                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_click" ValidationGroup="gi" class="btn btn-success" OnClientClick="return showdivimg();" />
                                        <asp:HiddenField ID="hdnmultiselectionid" runat="server" />

                                        <%--   <a href="" class="btn btn-success">Save</a>--%>
                                    </div>
                                    <script type="text/javascript">

                                        function showdivimg() {
                                            if (Page_ClientValidate("gi")) {
                                                if (confirm('Are You Sure You want to Submit This Entry!')) {
                                                    document.getElementById("pnlimg").style.display = "block";
                                                    document.getElementById("divsub").style.display = "none";
                                                    return true;
                                                }
                                                else {
                                                    return false;
                                                }
                                            }
                                        }
                                    </script>
                                </center>
                            </td>
                        </tr>
                    </table>


                    <script type="text/javascript">
                        $(function () {

                            if (document.getElementById("<%=hdnmultiselectionid.ClientID %>").value != "") {

                 var arrmul = document.getElementById("<%=hdnmultiselectionid.ClientID %>").value.split(',');
                                if (arrmul.length > 0) {
                                    for (var x = 0; x < arrmul.length; x++) {

                                        $('[id*=' + arrmul[x] + ']').multiselect({
                                            includeSelectAllOption: true
                                        });
                                    }
                                }
                            }
                        });

                        var prm1 = Sys.WebForms.PageRequestManager.getInstance();
                        if (prm1 != null) {
                            prm1.add_endRequest(function (sender, e) {
                                if (sender._postBackSettings.panelsToUpdate != null) {



                                    if (document.getElementById("<%=hdnmultiselectionid.ClientID %>").value != "") {

                         var arrmul = document.getElementById("<%=hdnmultiselectionid.ClientID %>").value.split(',');
                                        if (arrmul.length > 0) {
                                            for (var x = 0; x < arrmul.length; x++) {

                                                $('[id*=' + arrmul[x] + ']').multiselect({
                                                    includeSelectAllOption: true
                                                });

                                            }


                                        }
                                    }

                                }
                            });
                        };

                    </script>


                </div>



                    <asp:HiddenField ID="hdnreload11"  runat="server" />      
                        <script type="text/javascript">
                            if (document.getElementById("<%=hdnreload11.ClientID %>").value == "11") {
                                  parent.window.document.getElementById("ContentPlaceHolder1_btnclosen").click();
                                parent.document.getElementById("closepop").click();
                         
                            }
                          
                        </script>
    </form>


    <!--/PAGE -->
    <!-- JAVASCRIPTS -->
    <!-- Placed at the end of the document so the pages load faster -->
    <!-- JQUERY -->
    <script src="/js/jquery/jquery-2.0.3.min.js"></script>
    <!-- JQUERY UI-->
    <script src="/js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.min.js"></script>
    <!-- BOOTSTRAP -->
    <script src="/bootstrap-dist/js/bootstrap.min.js"></script>


    <!-- DATE RANGE PICKER -->
    <script src="/js/bootstrap-daterangepicker/moment.min.js"></script>

    <script src="/js/bootstrap-daterangepicker/daterangepicker.min.js"></script>
    <!-- SLIMSCROLL -->
    <!--<script type="text/javascript" src="js/jQuery-slimScroll-1.3.0/jquery.slimscroll.min.js"></script>
	
	<script type="text/javascript" src="js/jQuery-slimScroll-1.3.0/jquery.slimscroll.min.js"></script><script type="text/javascript" src="js/jQuery-slimScroll-1.3.0/slimScrollHorizontal.min.js"></script>-->
    <!-- BLOCK UI -->
    <script type="text/javascript" src="/js/jQuery-BlockUI/jquery.blockUI.min.js"></script>
    <!-- SPARKLINES -->
    <script type="text/javascript" src="/js/sparklines/jquery.sparkline.min.js"></script>
    <!-- EASY PIE CHART -->
    <script src="/js/jquery-easing/jquery.easing.min.js"></script>
    <script type="text/javascript" src="/js/easypiechart/jquery.easypiechart.min.js"></script>
    <!-- FLOT CHARTS -->
    <script src="/js/flot/jquery.flot.min.js"></script>
    <script src="/js/flot/jquery.flot.time.min.js"></script>
    <script src="/js/flot/jquery.flot.selection.min.js"></script>
    <script src="/js/flot/jquery.flot.resize.min.js"></script>
    <script src="/js/flot/jquery.flot.pie.min.js"></script>
    <script src="/js/flot/jquery.flot.stack.min.js"></script>
    <script src="/js/flot/jquery.flot.crosshair.min.js"></script>
    <!-- TODO -->
    <script type="text/javascript" src="/js/jquery-todo/js/paddystodolist.js"></script>
    <!-- TIMEAGO -->
    <script type="text/javascript" src="/js/timeago/jquery.timeago.min.js"></script>
    <!-- FULL CALENDAR -->
    <script type="text/javascript" src="/js/fullcalendar/fullcalendar.min.js"></script>
    <!-- COOKIE -->
    <script type="text/javascript" src="/js/jQuery-Cookie/jquery.cookie.min.js"></script>
    <!-- GRITTER -->
    <script type="text/javascript" src="/js/gritter/js/jquery.gritter.min.js"></script>
    <!-- CUSTOM SCRIPT -->
    <script src="js/script.js"></script>

   
    <!-- /JAVASCRIPTS -->


    <script type="text/javascript" src="/js/datatables/jquery.dataTables.js"></script>
    <!-- Data Tables -->
    <script type="text/javascript" src="/js/datatables/TableTools.js"></script>
    <!-- TableTools -->
    <script type="text/javascript" src="/js/datatables/jquery-editable/jquery.editable.js"></script>
    <!-- jQuery Editable -->
    <script type="text/javascript" src="/js/datatables/dataTables.editor.js"></script>
    <!-- Data Tables Editor-->
    <script type="text/javascript" src="/js/datatables/dataTables.editor.bootstrap.js"></script>
    <!-- Data Tables Editor for Bootstrap-->
    <script type="text/javascript" src="/js/datatables/dataTables.bootstrap.js"></script>
    <!-- Bootstrap Support for Datatables -->
    <script type="text/javascript" src="/js/datatables/demo-tableeditable.js"></script>
    <script src="/plugins/bootstrap-multiselect/bootstrap-multiselect.js" type="text/javascript"></script>




    <style>
        .trclass {
            cursor: pointer;
        }

            .trclass:hover {
                background: #CCC;
            }
    </style>
    <script type="text/javascript">
        function sendmainlink(id) {
            document.location.href = "groupsummary.aspx?id=" + id;

        }
        function sendsublink(id) {
            document.location.href = "ledgermonthlysummary.aspx?id=" + id;

        }

        function sendmonthlink(id, mid, yearid) {
            document.location.href = "ledgermonthlydetailssummary.aspx?id=" + id + "&mid=" + mid + "&yid=" + yearid;

        }
        function sendentrylink(id, entid, mid, yearid) {
            document.location.href = "editentry.aspx?id=" + id + "&entid=" + entid + "&mid=" + mid + "&yearid=" + yearid;

        }
        function showpage(pagename) {
            document.location.href = pagename;

        }
    </script>







  


   

</body>
</html>


