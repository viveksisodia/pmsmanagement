<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="StateMaster.aspx.cs" Inherits="settings_StateMaster" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="page-content">

        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico orange">
                                            <i class="ri-pencil-ruler-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=fns.getltext("State Master", "2")%></p>
                                           
                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                        <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                            <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addState"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">

                                <asp:Label ID="lblprojectdata" runat="server"></asp:Label>

                                <div style="display: none;">

                                    <asp:Button ID="btndelete" runat="server" Text="delete" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="offcanvas offcanvas-end" id="addState" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
        <div class="offcanvas-header border-bottom" style="padding: 14px 24px;">
            <h5 class="offcanvas-title" id="offcanvasRightLabel">
                <label id="lbltitle" runat="server"><%=fns.getltext("Add State  Master", "2")%></label>

            </h5>
            <div class="hstack gap-2">
                <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
                <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
                <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
                <div style="display: none">
                    <button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
                </div>
            </div>
        </div>
        <div class="offcanvas-body p-0 overflow-hidden">
            <div data-simplebar style="height: calc(100vh - 112px);">
                <div class="acitivity-timeline p-4">
                    <div class="row gy-4">
                        <div class="col-lg-12">
                            <div class="lead_form_body p-0">
                                <div class="row">
                                    <div style="display: none">
                                        <asp:Button ID="Button1" runat="server" Text="" OnClick="Button1_Click" />

                                    </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />


                                    <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("Country Name", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Countryid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                        </div>

                                          
                                       <cc1:DropDownListChosen ID="Countryid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Country ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>

                                       <%-- <asp:DropDownList ID="Countryid" CssClass="form-control" runat="server" width="450px" NoResultsText="No results match."
                                                    DataPlaceHolder="Country ..." AllowSingleDeselect="true"></asp:DropDownList>--%>

                                    </div>


                                      <div class="col-xxl-12 col-lg-12">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">
                                                <%=fns.getltext("State Name", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1"
                                                    ControlToValidate="Statename" ValidationGroup="btnsave"
                                                    runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                        </div>
                                        <input type="text" id="Statename" class="form-control" placeholder="State Name..." runat="server">
                                    </div>

                                    


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right">
            <div id="pnlimg" style="display: none;">
                <img src="/Image/loading8.gif" runat="server" id="img1" />
            </div>
            <div class="btn-group">

                <div class="btn-group" id="divsub" style="display: block;">
                    <a href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                    <%-- <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>--%>
                </div>
            </div>

            <a href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()" aria-label="Close">Close</a>
            <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display: none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        </div>
    </div>

    <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
        <div style="display: none;">
            <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
            <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
        </div>
        <iframe id='inner_popup' width="100%" height="100%" style="padding: 0px 0px;" border="0px"></iframe>
    </div>


    
    <%-- --------------------------------------------------------------------------------------------%>
    <script src="/pms/assets/libs/sortablejs/Sortable.min.js"></script>
    <script src="/pms/assets/js/pages/nestable.init.js"></script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css">
    <script src="/pms/assets/js/jquery-3.7.0.js"></script>
    <script src="/pms/assets/js/select2.min.js"></script>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jscolor.js"></script>

    <script>

  


    function setTextColor(picker) {
       // document.getElementsById(PrintingColor).value = '#' + picker.toString()
    }
</script>


    <script>
        function fsFunction() {
            var element = document.getElementById("addState");
            element.classList.add("canvas-fullscreen");
        }
        function fseFunction() {
            var element = document.getElementById("addState");
            element.classList.remove("canvas-fullscreen");
        }
    </script>

    <script>
        const table = new DataTable('.datatable', {
            scrollX: false,
            scrollCollapse: true,
            paging: true,
            searching: true

        });
    </script>
   <asp:HiddenField ID="hdnsavetype" runat="server" />
    <%--save data end  --%>

    <script type="text/javascript">
        function showdivimg() {
            if (Page_ClientValidate("btnclose")) {
                if (confirm('Do you want to Leave this page ?')) {

                    Cleardata();
                    document.getElementById("btnclose").click();

                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function SvaeData(val1A) {
            debugger
            if (Page_ClientValidate("btnsave")) {
                document.getElementById('<%=hdnsavetype.ClientID%>').value = val1A;

                if (confirm('Are You Sure You want to Submit This Entry!')) {
                    document.getElementById("pnlimg").style.display = "block";
                    document.getElementById("divsub").style.display = "none";

                }
                else {
                    return false;
                }





                // save all details


                var Statedetails = {};

                Statedetails.id = document.getElementById('<%=hdnid.ClientID%>').value;
                Statedetails.Countryid = document.getElementById('<%=Countryid.ClientID%>').value;
                Statedetails.StateName = document.getElementById('<%=Statename.ClientID%>').value;
                $.ajax({
                    type: "POST",
                    url: '/pms/settings/StateMaster.aspx/SaveStateMaster',
                    data: '{Statedetails: ' + JSON.stringify(Statedetails) + '}',
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessstatemaster,
                    error: function (response) {
                        debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        debugger
                        alert(response.responseText);
                    }

                });
                return false;



            };
        }
        function OnSuccessstatemaster(response) {
            debugger
            //commntprt
            alert("Data Saved successfully.");
             document.getElementById('<%=Countryid.ClientID%>').value = "0";
             document.getElementById('<%=Statename.ClientID%>').value = "";
          
            if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save") {

                document.getElementById("pnlimg").style.display = "none";
                document.getElementById("divsub").style.display = "block";
                document.getElementById("btncls").click();
                Bindcountry_data();
            }

            if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore") {
                document.getElementById("pnlimg").style.display = "none";
                document.getElementById("divsub").style.display = "block";
                Bindcountry_data();
            }



        }



        function Cleardata() {
            debugger;
            
              document.getElementById('<%=Countryid.ClientID%>').value = "0";
             document.getElementById('<%=Statename.ClientID%>').value = "";
          
            <%--document.getElementById('<%=Button1.ClientID%>').click();--%>
            document.getElementById('<%=lbltitle.ClientID%>').innerHTML = "Add State Mater";


        }

        function Binddetailsstatemaster(RecordId) {


            debugger;

            $.ajax({
                url: '/pms/settings/StateMaster.aspx/BindStatedetail',
                data: "{ 'RecordId': '" + RecordId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: OnSuccesscountry,
                error: function (response) {
                    //alert(response.responseText);
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });

        }


        function OnSuccesscountry(response) {

            debugger
            try {
                var printingdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (printingdetails.find("stateid").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = printingdetails.find("stateid").text();
                    document.getElementById('<%=Countryid.ClientID%>').value = printingdetails.find("conid").text();
                    document.getElementById('<%=Statename.ClientID%>').value = printingdetails.find("state").text();
                    document.getElementById('btnsave').value = "Update";
                    document.getElementById('<%=lbltitle.ClientID%>').innerHTML = "Edit State Master";
                }
            }
            catch
            {
            }

        }


    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        /* errorprt*/
        var jq = jQuery.noConflict();


        jq(document).ready(function () {
            BindState_data();
        });
        function BindState_data() {

            debugger;
            $.ajax({
                url: '/pms/settings/StateMaster.aspx/bindStateMaster',
                data: '{}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: OnSuccessState,
                error: function (response) {

                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });

        }
        function OnSuccessState(response) {
            debugger
            var todolist = response.d;
            let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
            lbl.innerHTML = '';
            lbl.innerHTML = todolist;

            /*errorprt*/

            table = new DataTable('.datatable', {
                scrollX: true,
                scrollCollapse: true,
                paging: true,
                searching: true

            });

        }

    </script>



    <script>
        function deletedata(RecordId) {
            debugger

            if (confirm('Do you really want to delete the Entry?')) {

            }
            else {
                return false;
            }

            $.ajax({
                url: '/pms/settings/StateMaster.aspx/DeleteStateMaster',
                data: "{ 'RecordId': '" + RecordId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: OnSuccessdelete,
                error: function (response) {
                    //alert(response.responseText);
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });

        }

        function OnSuccessdelete(response) {
            alert('Data Deleted !');
            BindState_data();
        }

    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
</asp:Content>







