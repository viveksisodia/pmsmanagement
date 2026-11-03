<%@ Page Title="" Language="C#" MasterPageFile="~/popup.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="viewsalesorder.aspx.cs" Inherits="viewsalesorder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/ucontrols/DateSelector.ascx" TagName="uc1" TagPrefix="uc1" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        table tr td {
            padding: 5px;
            vertical-align: top;
        }

            table tr td input, table tr td select, table tr td textarea {
                border: 1px solid #ccc;
                padding: 3px;
                width: 100%;
            }

            table tr td label {
                display: block;
                margin: 0px;
                padding: 0px;
            }

        ul.preferences {
            margin: 0px;
            padding: 0px;
            width: 100%;
            height: 100%;
            width: 100%;
            float: left;
        }

            ul.preferences li {
                margin: 0px;
                padding: 0px;
                list-style: none;
                width: 100%;
                float: left;
            }

                ul.preferences li a {
                    margin: 0px;
                    padding: 5px;
                    display: block;
                }

        .templates {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            -ms-flex-direction: row;
            flex-direction: row;
            -ms-flex-pack: start;
            justify-content: flex-start;
            margin-top: 20px;
        }

            .templates > li {
                float: left;
                margin: 0 14px 40px 0;
                width: 302px;
                height: 427px;
            }

            .templates .thumbnail {
                position: relative;
                height: 100%;
                width: 100%;
                padding: 0;
            }

            .templates.settings .caption {
                position: absolute;
                top: 0;
                left: 0;
                opacity: 0;
                width: 100%;
                height: 100%;
            }

            .templates.settings .thumbnail:hover object {
                opacity: .2
            }

            .templates.settings .thumbnail:hover .caption {
                opacity: 1
            }

            .templates.settings .thumbnail:hover img {
                opacity: .1
            }

            .templates.settings .caption .bt-actions {
                position: absolute;
                bottom: 0;
                left: 0;
                text-align: center;
                width: 100%;
                padding: 10px 0;
                background: rgba(0,0,0,.46);
            }

        .list-header {
            position: fixed;
            left: 60px;
            bottom: 0;
            width: 420px;
        }

            .list-header .list-filter {
                padding: 10px 10px 10px;
                border-bottom: 1px solid #e4e4e4;
                background: #eee;
                width: 100%;
                display: block;
            }

        .nav-temptab-column {
            width: 60px;
            z-index: 2;
            overflow: hidden;
            background-color: #ededed;
            height: 100%;
            border-right: 1px solid #e4e4e4;
            position: fixed;
            top: 60px;
            left: 0;
            z-index: 99;
        }

        .clogo {
            height: 50px;
        }

        ul.edit-center {
            margin: 0px;
            padding: 0px;
            list-style: none;
            width: 100%;
            float: left;
        }

            ul.edit-center li {
                width: 100%;
                padding: 8px 5px;
                text-align: center;
                font-size: 12px;
                line-height: 12px;
                color: #333;
            }

                ul.edit-center li a {
                    color: #333;
                }

                ul.edit-center li:hover {
                    background: #f8f8f8;
                }

                ul.edit-center li.active a {
                    color: #fff;
                }

                ul.edit-center li.active > a svg.lpanel {
                    fill: #fff;
                }

                ul.edit-center li.active {
                    background: #14bdbf;
                }

                    ul.edit-center li.active:hover {
                        background: #14bdbf;
                    }

                ul.edit-center li .icontab {
                    padding: 0px 15px;
                    display: block;
                }

        .nav-temptab-content {
            width: 495px;
            z-index: 9;
            background: #fff;
            padding-left: 60px;
            padding-bottom: 30px;
            margin: 0px;
            position: fixed;
            height: 100%;
            top: 60px;
            bottom: 0;
            left: 0;
            overflow: auto;
        }

        .tab-content {
            width: 100%;
            float: left;
            padding-bottom: 130px;
            min-height: 700px;
        }

        .nav-temptab-content h4 {
            border-bottom: 1px solid #ccc;
            margin: 0px;
            padding: 21px 15px 20px 15px;
            display: block;
            font-size: 16px;
        }

        .inner-content {
            width: 100%;
            height: 100%;
        }

        .mb-10 {
            width: 100%;
            float: left;
            margin-bottom: 10px;
        }

        .input-group[class*="col-"] {
            padding-right: 15px;
        }

        .pl-0 {
            padding-left: 0px;
        }

        .form-group {
            width: 100%;
            float: left;
            margin-bottom: 10px;
        }

        .input-group-addon button {
            border: 1px solid #ccc;
        }

        .radibuttonlistclass input {
            width: 15px;
            margin-right: 5px;
        }

        .radibuttonlistclass label {
            display: inline-block;
        }
    </style>
      <style>
                #myList {
                    margin-top: 10px;
                    margin-bottom: 0px;
                }

                    #myList li {
                        display: none;
                    }
            </style>
    <script>
        function hidemaster()
                                              {
                                                  debugger
                                                  if (document.getElementById('dvactivity').style.display == 'none') {
                                                      document.getElementById('dvactivity').style.display = 'block';
                                                  }
                                                  else {
                                                      document.getElementById('dvactivity').style.display = 'none';
                                                  }
                                                   
                                              }
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
    <script type="text/javascript" language="javascript">
         function close_bycode()
         {
             try {
                 window.parent.document.getElementById("btnmaster").click();
             }
             catch
             {}
        }
        function closepopupsaleordermain()
        {        
            debugger
         close_bycode();
         try
         {
            window.parent.document.getElementById("dvmangesale").style.display = 'block';
         }
        catch
         { }
          try
         {
            window.parent.document.getElementById("dv_appso").style.display = 'block';
           
         }
        catch
        { }
         try
         {
             window.parent.document.getElementById("closenewsale").click();
             
         }
         catch
         {
             //window.parent.document.getElementById("btnclosemainpopup").click();
         }
            try {
            window.parent.document.getElementById("btnclosemainpopup").click();
            }
            catch
            {
                
            }          
        }

</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnreload" runat="server" />
    <script>
        if (document.getElementById("<%=hdnreload.ClientID %>").value == "1") {
            debugger
            parent.window.document.location.reload();
            closepopupsaleordermain();
           
           }
       
    </script>
    <%--<h5 class="modal-title text-white pull-left"> Sales Order</h5>
            <div id="dv_viewsaleorder">
               <button type="button"  class="close"  onclick="closepopupsale();"> <span>x</span> </button>
             </div>--%>
    <div class="clearfix" id="dvactivity">
                                        <header style="background: #72075d;width:100%;height: 46px;padding: 15px;float:left;margin-bottom:10px;">
										<h5 class="content-title pull-left" style="color:#fff;margin:0;">
                                            <asp:Label ID="Label1" runat="server" ><%=fns.getltext("View Sales Order", "2")%></asp:Label> 
                                       </h5>
            <div id="dv_viewsale">                            
          <button type="button"  class="close"  onclick="closepopupsaleordermain();"> <span>x</span> </button>
         </div>
        </header>
									</div>

    <div style="display: none;">
        <cc1:DropDownListChosen ID="DropDownListChosen1" runat="server" NoResultsText="No results match." Width="300px"
            DataPlaceHolder="Type Here..." AllowSingleDeselect="true">
        </cc1:DropDownListChosen>
    </div>


   
        <!-- STYLER -->

        <!-- BREADCRUMBS -->
        
        


        <div class="clearfix"  style="display:none;">
            <asp:Label ID="lblviewlogs" runat="server"></asp:Label>
             <asp:Label ID="lblallorder" runat="server" Text="All Sales Order"></asp:Label>
              <asp:Label ID="lbladdorder" runat="server" Text="+ New Sales Order"></asp:Label>
            <h3 class="content-title pull-left">
                <asp:Label ID="lbltitle" runat="server"></asp:Label>
            </h3>

        </div>

         <%-- code here--%>
        <div class="col-md-12" style="padding-left: 0px; padding-right: 0px; display:none;">
         <ul class="demo-btns" style="margin:0px 4px 0 10px;">
             <li><asp:LinkButton ID="lnkreccived" runat="server" onclick="lnkreccived_Click" CssClass="btn btn-success" title="Click To Send Items" ><%=fns.getltext("Click To Send Items", "2")%></asp:LinkButton></li>
             
             <%--<li> <a href="/addnewinvoiceordersalesorder.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales" class="btn btn-success" title="Click To Send Items">Click To Send Items</a></li>--%>
             
             <li>
                  <asp:Label ID="lblviewhistory" runat="server">
                 <a data-toggle="modal" data-target="#myModalassetsemail" href="javascript:void(0);"  class="btn btn-primary" title="View History of Invoice" onclick="openSrc('myModalassetsemailsrc','historyofsalesorder.aspx?id=<%=Request.QueryString["id"].ToString() %>','<%=fns.getltext("View History of Invoice", "2")%>')"><%=fns.getltext("View History of Invoice", "2")%></a>
                 </asp:Label>
             </li>
            
             <li> 
                 <asp:Panel ID="pnlpaymentschedule" runat="server" Visible="false">
                 <a data-toggle="modal" data-target="#myModalassetsemail" href="javascript:void(0);"  class="btn btn-inverse" title="Payment Schedule" onclick="openSrc('myModalassetsemailsrc','managepaymentschedule.aspx?id=<%=Request.QueryString["id"].ToString() %>','<%=fns.getltext("Payment Schedule", "2")%>')"><%=fns.getltext("Payment Schedule", "2")%></a>
                 </asp:Panel>
             </li>

                <li><asp:LinkButton ID="lnkreccivedDelivery" runat="server" onclick="lnkreccivedDelivery_Click" CssClass="btn btn-success" title="Add DC" ><%=fns.getltext("Add DC", "2")%></asp:LinkButton></li>
                <li><asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" CssClass="btn btn-success" title="Add Invoice From DC" ><%=fns.getltext("Add Invoice From DC", "2")%></asp:LinkButton></li>

             <%--<li> <a href="/addnewinvoiceorderDeliveryorder.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales" class="btn btn-info" title="Add DC">Add DC</a></li>
             <li> <a href="/manageInvoicesdeliveryorderforinvoice.aspx?poid=<%=Request.QueryString["id"].ToString() %>" class="btn btn-warning" title="Add Invoice From DC">Add Invoice From DC</a></li>--%>
             
             <li> <a data-toggle="modal" data-target="#myModalassetsemaildo" href="javascript:void(0);"  class="btn btn-danger" title="View History of DC" onclick="openSrc('myModalassetsemaildosrc','historyofsalesorderdo.aspx?id=<%=Request.QueryString["id"].ToString() %>','<%=fns.getltext("View History of DC", "2")%>')"><%=fns.getltext("View History of DC", "2")%></a></li>
             </ul>
        </div>
       <div class="col-md-12">
       
           <asp:Panel ID="pnldelete" runat="server" Visible="false"></asp:Panel>
        <asp:Panel ID="pnlapprove1" runat="server">
             <ul class="demo-btns">
            <li><a href="#" id="liid" rel="popover" data-popover-content="#myPopover" data-placement="bottom" class="btn btn-success"><i class="fa fa-paperclip"></i>
                <asp:Label ID="filecount" runat="server"></asp:Label></a>&nbsp;&nbsp;</li>
        </ul>

        <div id="myPopover" class="hide attachments-invoice">
            <div class="list-primary" id="attachedlist">
                <asp:UpdatePanel ID="updatepanel" runat="server">

                    <ContentTemplate>
                        <div style="display: none;">

                            <asp:Button ID="btndeleteattament" runat="server" Text="deleteatta" OnClick="btndeleteattament_click" />
                            <asp:HiddenField ID="hdnattid" runat="server" />

                        </div>
                        <%=attfilesdata %>



                        <script type="text/javascript">

                            function deleteatt(val, valcal) {

                                if (confirm('Are You Sure To Delete This File?')) {


                                    document.getElementById("<%=hdnattid.ClientID %>").value = val;

            $("#<%=btndeleteattament.ClientID %>").click();
            $("#liid").click();
            //alert('File Deleted Successfully!!');
            document.getElementById("<%=filecount.ClientID %>").innerHTML = valcal;
                                }

                            }

                        </script>


                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="list-item">
                    <div class="font-xs text-center">
                        <%=fns.getltext("You can upload a maximum of 5 files, 5MB each", "2")%>
                    </div>
                    <table cellpadding="0" cellspacing="5" class="text-center font-xs upload-options" width="100%">
                        <tbody>
                            <tr>
                                <td class="drag-drop-bg" width="30%">
                                    <%-- <div class="file-upload">
                  <input multiple class="upload" type="file">
                  <div><i class="fa fa-upload"></i>
              </div>
              <a>Drag & drop</a> </div>--%>

                                    <a href='#' id='uploadfiletag' onclick="openUploadFile()"><%=fns.getltext("Upload File", "2")%></a>
                                </td>

                            </tr>
                        </tbody>
                    </table>

                    <div style="display: none;">
                        <asp:FileUpload ID="fileuploaderfile" runat="server" Width="65px" ToolTip="Upload Logo" onchange="UploadFile()" Style="display: inline-block;" />
                        <asp:Button ID="btnuploadfile" runat="server" OnClick="btnupload_click" Text="upload" />
                    </div>
                </div>

                <script lang="javascript" type="text/javascript">

                    function UploadFile() {


                        var value = $("#<%=fileuploaderfile.ClientID %>").val();
           if (value != '') {


               $("#<%=btnuploadfile.ClientID %>").click();

                        }
                    };
                    function openUploadFile() {


                        $("#<%=fileuploaderfile.ClientID %>").click();
                    };
                </script>

            </div>
        </div>
        <script>
            $(function () {
                $('[rel="popover"]').popover({
                    container: 'body',
                    html: true,
                    content: function () {
                        var clone = $($(this).data('popover-content')).clone(true).removeClass('hide');
                        return clone;
                    }
                }).click(function (e) {
                    e.preventDefault();
                });
            });
        </script>
        <script>

            var count = 1;
            function openSrc(val1, val2, val3) {
                //alert(val3);
                count++;
                document.getElementById(val1).src = "";
                document.getElementById(val1).src = val2 + "&count=" + count;
                try {

                    document.getElementById("<%=lbltitlepopup.ClientID%>").innerHTML = val3;
                }
                catch (ex) {
                }
            }
 function showclosebtn() 
                        {
                       debugger
                        try {
                       document.getElementById("dv_viewsale").style.display = "Block";
                        }
                        catch (ex)
                        {
                        }
                       document.getElementById("closepop").click();
                      }
        </script>

        <div class="modal fade in inner-modal" id="myModalassetsemail" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog" style="width: 100%; margin: 0px; height: 100%;">
                <div class="modal-content" style="width: 100%; height: 100%;">
                     <button type="button" class="close" onclick="showclosebtn();"><span>x</span> </button>
                    <button type="button" id="closepop" style="display:none" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper" style="width: 100%; height: 100%;">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0px;"><asp:Label ID="lbltitlepopup" runat="server"></asp:Label></h5>
          </div>
        </header>
                        
                        <iframe id='myModalassetsemailsrc' width="100%" height="100%" style="padding: 0px 0px;border:0px;" border="0px"></iframe>
                    </div>

                </div>
            </div>
        </div>
        <div class="modal fade in inner-modal" id="myModalassetsemaildo" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog" style="width: 100%; margin: 0px; height: 100%;">
                <div class="modal-content" style="width: 100%; height: 100%;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper" style="width: 100%; height: 100%;">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;">
                <%=fns.getltext("View History of DC","2") %> 
            </h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <iframe id='myModalassetsemaildosrc' width="100%" height="95%" style="padding: 0px 0px;" border="0px"></iframe>
                    </div>
                </div>
            </div>
        </div>

            <ul class="demo-btns">
                <%if (iscancel == "0" && module1update == 1)
                    { %>
                <%if (Request.QueryString["typesale"] != null)
                    { %>
                <li>
                    <%--<a href="/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>&typesale=<%=Request.QueryString["typesale"].ToString() %>" class="btn btn-info" title="Edit">--%>
                   <a href="javascript:void(0);"  data-toggle="modal" data-target="#editsaleorder"   class="btn btn-info" title="Edit" 
                    onclick="loadsrcedit_saleorder('myModalsaleorderedit','/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>&typesale=<%=Request.QueryString["typesale"].ToString() %>&calltype=<%=cmf.Encrypt("crm")%>','<%=fns.getltext("Edit Sale Order", "2")%>')">
                        
                        <i class="fa fa-pencil"></i></a></li>

                <%}
                else
                { %>
                <li>
                   <%-- <a href="/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>" class="btn btn-info" title="Edit">--%>
                        <a href="javascript:void(0);"  data-toggle="modal" data-target="#editsaleorder"   class="btn btn-info" title="Edit" 
                    onclick="loadsrcedit_saleorder('myModalsaleorderedit','/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>&calltype=<%=cmf.Encrypt("crm")%>','<%=fns.getltext("Edit Sale Order", "2")%>')">
                        <i class="fa fa-pencil"></i></a></li>



                <%} %>
                <%} %>

                <% if (module1view == 1 && isapproveshow == 1)
                    {%>
                <li><a href="/downloadsalespdf.aspx?strurldata=<%=strurldata %>" class="btn btn-danger" title="PDF"><i class="fa fa-file-pdf-o"></i></a></li>
                <li>
                    <a href="https://web.whatsapp.com/send?phone=+91<%=customermobile %>" target="_blank" class="btn btn-success btn-whatsapp"><i class="fa fa-whatsapp"></i></a>
                    <a href="https://api.whatsapp.com/send?phone=+91<%=customermobile %>" target="_blank" class="btn btn-success btn-whatsapp2"><i class="fa fa-whatsapp"></i></a>
                </li>


                <li><a role="button" data-toggle="modal" data-target="#viewpdf" href="javascript:void(0);" onclick="hidemaster()" title="Print" class="btn btn-primary"><i class="fa fa-print"></i></a></li>
                <li><a role="button" data-toggle="modal" data-target="#viewpdfonletterhead" href="javascript:void(0);" onclick="hidemaster()" title="Print on Letter Head" class="btn btn-primary"><i class="fa fa-print"></i> <%=fns.getltext("Print On Letterhead", "2")%></a></li>

                <%} %>
                <% if (module1update == 1 && isapproveshow == 1)
                    {%>
                <li><a data-toggle="modal" data-target="#myModalassetsemail" href="javascript:void(0);"   class="btn btn-warning" title="Email" onclick="loadsrc('myModalassetsemailsrc','emailsalesorder.aspx?id=<%=Request.QueryString["id"].ToString() %>')"><i class="fa fa-envelope-o"></i></a></li>



                <script>

                    var count = 1;
                    function loadsrc(val1, val2) {
                        count++;
                        //  alert(val1);
                        document.getElementById(val1).src = val2 + "&count=" + count;
                        try {
                       document.getElementById("dv_viewsale").style.display = 'none';
                        }
                        catch (ex) {
                        }
                    }

                    
                </script>
                <div class="modal fade in inner-modal" id="myModalassetsemail" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
                    <div class="modal-dialog" style="width: 100%; margin: 0px; height: 100%;">
                        <div class="modal-content" style="width: 100%; height: 100%;">
                            
                            <button type="button" id="btncls"  class="close" data-dismiss="modal" aria-hidden="true"  ><span>x</span> </button>
                            <div id="Div1" style="width: 100%; height: 100%;">
                                <header id="Header1" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar">
            <h5><%=fns.getltext("Send Email", "2")%></h5>
          </div>
        </header>
                                <br />
                                <br />
                                <br />
                                <br />


                                <iframe id='myModalassetsemailsrc' width="100%" height="95%" style="padding: 0px 0px;" border="0px"></iframe>
                            </div>

                        </div>
                    </div>
                </div>



                <%} %>




                 <%--&& isapproveshow == 1--%>
                <%if (iscancel == "0")
                    { %>

                <li>
                    <asp:Panel ID="pnlconvertproduction" runat="server">

                        <%-- <a href="javascript://" class="btn btn-default" onclick="return openqty('<%=Request.QueryString["id"] %>')">View BOM</a>
                        --%>
                    </asp:Panel>
                </li>
                <script language="javascript">

                    function deletconfirm() {
                        if (confirm('<%=fns.getltext("Do you really want to Converted into Production Of This Sales Order?","2") %>')) {
                            return true;
                        }
                        return false;


                    }
                </script>






                <li>
                    
                    <asp:Panel ID="pnlinvoice" runat="server">
                       
                        <div class="btn-group">
                            <button type="button" class="btn btn-inverse dropdown-toggle" data-toggle="dropdown">
                                <%=fns.getltext("Convert", "2")%> <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">

                                <%--&& ddlproformainvoice == 2 --%>
                                <%if (iscancel == "0" && proformainvoicecreate == 1 )
                                    { %>
                                <li>

                                   <%-- <a href="/crm/addnewProformaInvoice.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales"><%=fns.getltext("Convert to Proforma Invoice", "2")%></a>--%>
                                   <a href="javascript:void(0);"  data-toggle="modal" data-target="#editsaleorder"  
                    onclick="loadsrcedit_saleorder('myModalsaleorderedit','/crm/addnewProformaInvoice.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales','<%=fns.getltext("Convert to Proforma Invoice", "2")%>')">
         <asp:Label ID="Label3" runat="server" Text=""><%=fns.getltext("Convert to Proforma Invoice", "2")%></asp:Label>
                 </a>
                                </li>
                                <%} %>
                                <%if (iscancel == "0" && mainvoicecreate == 1&&isinvoiced==0)
                                    { %>
                                <li>

                                    <a href="javascript:void(0);"  data-toggle="modal" data-target="#editsaleorder"  
                    onclick="loadsrcedit_saleorder('myModalsaleorderedit','/crm/addnewinvoiceordersalesorder.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales','<%=fns.getltext("Convert to Invoice", "2")%>')">
                  <%=fns.getltext("Convert to Invoice", "2")%>
                 </a>
                                    <%--<a href="/crm/addnewinvoiceorderclonefromsalesorder.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=sales"><%=fns.getltext("Convert to Invoice", "2")%></a>--%>


                                </li>

                                <%} %>
                            </ul>
                        </div>


                    </asp:Panel>
                </li>
                <%} %>


                <%if (module1update == 1 || module1create == 1 || module1delete == 1)
                    {%>
                <li>
                    <div class="btn-group">
                        <button type="button" class="btn btn-inverse dropdown-toggle" data-toggle="dropdown">
                            <%=fns.getltext("More", "2")%> <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" role="menu">
                            <%if (isdutyslip == 1)
                                {%>
                            <li><a role="button" href="/printdutyslip.aspx?id=<%=Request.QueryString["id"].ToString() %>" target="_blank" title="Print Duty Slip"><%=fns.getltext("Print Duty Slip", "2")%></a></li>



                            <%} %>
                            <%if (isdutysliprajshree == 1)
                                {%>
                            <li><a role="button" href="/printdutysliprajshree.aspx?id=<%=Request.QueryString["id"].ToString() %>" target="_blank" title="Print Duty Slip"><%=fns.getltext("Print Duty Slip", "2")%></a></li>



                            <%} %>

                            <%if (islock == "1" && islock1 == "1")
                                { %>
                            <li><a role="button" data-toggle="modal" data-target="#lockdiv" href="javascript:void(0);"><%=fns.getltext("Save & Lock", "2")%></a></li>
                            <%} %>
                            <%if (iscancel == "0" && module1update == 1)
                                { %>

                            <li><a role="button" data-toggle="modal" data-target="#canceldiv" href="javascript:void(0);"><%=fns.getltext("Mark as Canceled", "2")%></a></li>
                            <li><a role="button" data-toggle="modal" data-target="#canceldivitem" href="javascript:void(0);"><%=fns.getltext("Cancel Items", "2")%></a></li>

                            <%} %>

                            <% if (module1create == 1)
                                {%>
                           <%-- addnewsalesorderclone--%>
                            <li>
                               
                                <%--<a href="/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=self"><%=fns.getltext("Clone", "2")%></a>--%>
                                 <a href="javascript:void(0);"  data-toggle="modal" data-target="#editsaleorder"  
                    onclick="loadsrcedit_saleorder('myModalsaleorderedit','/crm/addnewsalesordersinglecal.aspx?id=<%=Request.QueryString["id"].ToString() %>&type=self','<%=fns.getltext("Clone", "2")%>')">
         <asp:Label ID="Label5" runat="server" Text=""><%=fns.getltext("Clone", "2")%></asp:Label>
                 </a>


                            </li>
                            <%} %>
                            <% if (module1delete == 1)
                                {%>
                            <li class="divider"></li>
                            <li><a role="button" data-toggle="modal" data-target="#deletediv" href="javascript:void(0);"><%=fns.getltext("Delete", "2")%></a></li>
                            <%} %>
                        </ul>
                    </div>
                </li>

                <%} %>

           </ul>





        </asp:Panel>
</div>

       






        <div class="nav-temptab-content" style="display: none;">

            <div class="tab-content">
                <div id="home" class="tab-pane fade in active">
                    <h4>Template Properties</h4>
                    <div class="" style="margin-top:15px">
                        <div class="col-md-12">
                            <label class="control-label required">Template Name</label>
                            <input class="form-control" type="text" id="Teamplatename" runat="server" />
                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">Paper Size</label>
                            <div>
                                <label>

                                    <asp:RadioButtonList ID="PaperSize" runat="server" RepeatDirection="Horizontal" CssClass="radibuttonlistclass">
                                        <asp:ListItem>A4</asp:ListItem>
                                        <asp:ListItem>letter</asp:ListItem>
                                    </asp:RadioButtonList>

                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">Orientation</label>
                            <div>
                                <label>

                                    <asp:RadioButtonList ID="Orientation" runat="server" RepeatDirection="Horizontal" CssClass="radibuttonlistclass">
                                        <asp:ListItem>Portrait</asp:ListItem>
                                        <asp:ListItem>Landscape</asp:ListItem>
                                    </asp:RadioButtonList>

                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">Margins <span class="text-muted">(in inches)</span></label>
                            <div class="row">
                                <div class="col-md-3">
                                    <input class="form-control" type="text" id="MarginsTop" runat="server" />
                                    <small>Top</small>
                                </div>
                                <div class="col-md-3">
                                    <input class="form-control" type="text" id="MarginsBottom" runat="server" />
                                    <small>Bottom</small>
                                </div>
                                <div class="col-md-3">
                                    <input class="form-control" type="text" id="MarginsLeft" runat="server" />
                                    <small>Left</small>
                                </div>
                                <div class="col-md-3">
                                    <input class="form-control" type="text" id="MarginsRight" runat="server" />
                                    <small>Right</small>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">PDF Font</label>
                            <div id="ember1535" class="ember-view">
                                <div class="ac-box ">
                                    <asp:DropDownList ID="PDFFont" runat="server" CssClass="ac-selected form-control">

                                        <asp:ListItem>Open Sans</asp:ListItem>
                                        <asp:ListItem>Helvetica</asp:ListItem>
                                        <asp:ListItem>Noto Sans</asp:ListItem>
                                        <asp:ListItem>DejaVu Sans</asp:ListItem>
                                        <asp:ListItem>DejaVu LGC Sans</asp:ListItem>
                                        <asp:ListItem>IPAPGothic</asp:ListItem>
                                        <asp:ListItem>AR PL New Sung</asp:ListItem>
                                        <asp:ListItem>Baekmuk Gulim</asp:ListItem>
                                        <asp:ListItem>Ubuntu</asp:ListItem>
                                        <asp:ListItem>Roboto</asp:ListItem>
                                    </asp:DropDownList>

                                </div>

                            </div>
                            <small class="help-block"></small>
                        </div>
                    </div>
                    <%-- <div class="">
                        <div class="col-md-12">
                          <label class="control-label"> <a>Add Attention Content</a></label>
                        </div>
                      </div>--%>

                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">
                                Background Image 
                            </label>
                            <div>
                                <asp:Panel ID="pnlbackgroundupload" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="fileupload" runat="server" CssClass="file-upload" ClientIDMode="Static" onchange="UploadFile()" />


                                            </td>
                                            <td>
                                                <asp:Button ID="uploadfilebtn" Text="Upload" runat="server" />
                                            </td>
                                        </tr>
                                    </table>


                                </asp:Panel>
                                <%--
                          <span style="padding: 5px 12px; width:100%;" bubbles="false" class="btn btn-default">
                            <input accept="image/gif,image/jpeg,image/png,image/bmp" class="upload" type="file">
                            </span> --%>
                            </div>
                            <asp:Panel ID="pnlbackground" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBackgroundImage" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:LinkButton ID="btBackgroundImagedel" runat="server"><img src="/images/delete.jpg" /></asp:LinkButton>

                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-12">
                            <label class="control-label">Image Position</label>
                            <div>
                                <div>
                                    <asp:DropDownList ID="imagepotion" runat="server" CssClass="form-control">

                                        <asp:ListItem>Top left</asp:ListItem>
                                        <asp:ListItem>Top center</asp:ListItem>
                                        <asp:ListItem>Top right</asp:ListItem>
                                        <asp:ListItem>Center left</asp:ListItem>
                                        <asp:ListItem>Center center</asp:ListItem>
                                        <asp:ListItem>Center right</asp:ListItem>
                                        <asp:ListItem>Bottom left</asp:ListItem>
                                        <asp:ListItem>Bottom center</asp:ListItem>
                                        <asp:ListItem>Bottom right</asp:ListItem>

                                    </asp:DropDownList>




                                    <div></div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="">
                        <div class="col-md-6">
                            <label class="control-label">Background Color</label>
                            <div>
                                <div class="col-md-2" style="padding: 0;">


                                    <input type="checkbox" runat="server" id="isbackgroundcolor" />
                                </div>
                                <div class="col-md-10">
                                    <div class="input-group row">
                                        <input class="form-control" id="BackgroundColor" value="000000" runat="server" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=BackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="control-label">Label Color</label>
                            <div class="input-group">
                                <input class="form-control" id="LabelColor" runat="server" value="000000" />
                                <span class="input-group-addon">
                                    <button class="jscolor {valueElement:'<%=LabelColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <div class="col-md-6">
                            <label class="control-label">Font Color</label>
                            <div id="ember1610" class="ember-view">
                                <div class="input-group">
                                    <input class="form-control" id="FontColor" runat="server" value="000000" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=FontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="control-label over-flow">Font Size</label>
                            <div class="input-group">
                                <input id="txtFontSize" runat="server" class="form-control ember-text-field ember-view" type="text" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="menu1" class="tab-pane fade">
                    <h4>Header</h4>
                    <div class="" style="margin-top: 15px;">
                        <div class="col-md-12">
                            <label class="control-label">Background Image </label>
                            <div>


                                <asp:Panel ID="pnlbackgroundupload1" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="fileupload1" runat="server" CssClass="file-upload" ClientIDMode="Static" onchange="UploadFile()" />


                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" Text="Upload" runat="server" />
                                            </td>
                                        </tr>
                                    </table>


                                </asp:Panel>

                            </div>
                            <asp:Panel ID="pnlbackground1" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBackgroundImage1" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:LinkButton ID="btBackgroundImagedel1" runat="server"><img src="/images/delete.jpg" /></asp:LinkButton>

                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>



                        </div>
                    </div>
                    <div class=" ">
                        <div class="col-md-12">
                            <label class="control-label">Image Position</label>
                            <div>
                                <asp:DropDownList ID="headerbackgroundimagepotion" runat="server" CssClass="form-control">

                                    <asp:ListItem>Top left</asp:ListItem>
                                    <asp:ListItem>Top center</asp:ListItem>
                                    <asp:ListItem>Top right</asp:ListItem>
                                    <asp:ListItem>Center left</asp:ListItem>
                                    <asp:ListItem>Center center</asp:ListItem>
                                    <asp:ListItem>Center right</asp:ListItem>
                                    <asp:ListItem>Bottom left</asp:ListItem>
                                    <asp:ListItem>Bottom center</asp:ListItem>
                                    <asp:ListItem>Bottom right</asp:ListItem>

                                </asp:DropDownList>
                                <div><i class="zf-ac-toggler"></i></div>
                            </div>
                        </div>
                    </div>

                    <div class="">
                        <div class="col-md-6">
                            <label class="control-label">Background Color</label>
                            <div>
                                <div class="col-md-2" style="padding: 0;">
                                    <input type="checkbox" id="isheaderBackgroundColor" runat="server" />
                                </div>
                                <div class="col-md-10">
                                    <div class="input-group row">
                                        <input class="form-control" id="headerBackgroundColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=headerBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--  <div class="col-md-12" style="margin:10px 0 5px 0px;"><a role="button" data-toggle="modal" data-target="#myModalform6"  href="javascript:void(0);" >Customize your header content</a></div>--%>
                    <div class="col-md-12">
                        <div class="checkbox">
                            <label class="control-label">
                                <input class="" type="checkbox" id="isfirstpage" runat="server" />
                                Apply to first page only
                            </label>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <label class="control-label"><strong>Document Title</strong></label>
                    </div>
                    <div>
                        <div class="col-md-5">
                            <div class="checkbox">
                                <label>
                                    <input class="" type="checkbox" id="isdocumenttitle" runat="server" />
                                    Title
                                </label>
                            </div>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="DocumentTitle" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Font Size</label>
                            <div class="col-md-7">
                                <div class="input-group">
                                    <input class="form-control" type="text" id="DocumentTitleFontSize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=" ">
                        <label class="control-label col-md-5">Font Color</label>
                        <div class="col-md-7">
                            <div class="ember-view">
                                <div class="input-group">
                                    <input class="form-control" id="DocumentTitleFontColor" runat="server" value="000000" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=DocumentTitleFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <label class="control-label"><strong>Organization</strong></label>
                    </div>

                    <div class="col-md-12">
                        <label>
                            <input class="" type="checkbox" id="ShowOrganizationLogo" runat="server" />
                            Show Organization Logo
                        </label>
                    </div>

                    <div class="col-md-12">
                        <label>
                            <input class="" type="checkbox" id="ShowOrganizationName" runat="server" />
                            Show Organization Name
                        </label>
                    </div>

                    <div class="col-md-12">
                        <label>
                            <input class="" type="checkbox" id="ShowOrganizationAddress" runat="server" />
                            Show Organization Address <i class="popovercontainer form-control-static ember-view" data-original-title="" title=""></i>
                        </label>
                    </div>

                    <div class=" ">
                        <div class="col-md-6">
                            <label class="control-label">Name Color</label>
                            <div class="ember-view">
                                <div class="input-group">
                                    <input class="form-control" id="OrganizationNameColor" runat="server" value="000000" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=OrganizationNameColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="control-label over-flow">Name Font Size</label>
                            <div class="input-group">
                                <input class="form-control " type="text" id="OrganizationNamefont" runat="server" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12" style="margin-top: 10px;">
                        <label class="control-label"><strong>Customer Name</strong></label>
                    </div>
                    <div class=" ">
                        <div class="col-md-6">
                            <label class="control-label">Font Color</label>
                            <div>
                                <div class="input-group">
                                    <input class="form-control" id="customerfontcolor" runat="server" value="000000" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=customerfontcolor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="control-label">Font Size</label>
                            <div class="input-group">
                                <input class="form-control" type="text" id="customerfontsize" runat="server" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>
                    <%--<div class="col-md-12" style="margin:10px 0 5px 0px;"> <a href="/">Customer Address Format</a></div>--%>
                    <div class="col-md-12">
                        <label class="control-label"><strong>Document Information</strong></label>
                    </div>
                    <div class="mb-10">
                        <label class="control-label col-md-5">Number Field</label>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="NumberField" runat="server" />
                        </div>
                    </div>

                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input class="" type="checkbox" id="isDateField" runat="server" />
                                Date Field
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="DateField" runat="server" />
                        </div>
                    </div>
                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input class="" type="checkbox" id="isExpiryDate" runat="server" />
                                Expiry Date
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="ExpiryDate" runat="server" />
                        </div>
                    </div>
                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input class="" type="checkbox" id="isReferenceField" runat="server" />
                                Reference Field
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="ReferenceField" runat="server" />
                        </div>
                    </div>
                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input class="" type="checkbox" id="isSalesperson" runat="server" />
                                Salesperson
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="Salesperson" runat="server" />
                        </div>
                    </div>
                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input class="" type="checkbox" id="isBillTo" runat="server" />
                                Bill To
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control " type="text" id="BillTo" runat="server" />
                        </div>
                    </div>

                    <div class="mb-10">
                        <div class="col-md-5">
                            <label>
                                <input type="checkbox" id="isShipTo" runat="server" />
                                Ship To
                            </label>
                        </div>
                        <div class="col-md-7">
                            <input class="form-control" type="text" id="ShipTo" runat="server" />
                        </div>
                    </div>

                </div>


                <div id="menu2" class="tab-pane fade">
                    <h4>Item Table</h4>

                    <div style="padding: 15px;">
                        <div class="btn-group">
                            <button type="button" id="btnlabels" class="btn btn-primary" onclick="showlabels()">Labels</button>
                            <button type="button" id="btnLayout" class="btn btn-default" onclick="showLayout()">Layout</button>
                        </div>
                    </div>


                    <div id="tclabels" style="display: block;">
                        <div style="border-bottom: 1px solid #eeeeee; width: 100%; float: left;" class="form-group">
                            <div class="col-md-5">
                                <label class="font-xs text-muted"></label>
                            </div>


                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="SerialNumber1" runat="server" />
                                        Serial Number
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="SerialNumber" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Item1" runat="server" />
                                        Item
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Item" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Description1" runat="server" />
                                        Description
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Description" runat="server" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Quantity1" runat="server" />
                                        Quantity
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Quantity" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Rate1" runat="server" />
                                        Rate
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Rate" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Taxpercent1" runat="server" />
                                        Tax(%)
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Taxpercent" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="TaxAmount1" runat="server" />
                                        Tax Amount
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="TaxAmount" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Discount1" runat="server" />
                                        Discount
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Discount" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Amount1" runat="server" />
                                        Amount
                                    </label>
                                </div>
                            </div>

                            <div class="col-md-4 pl-0">
                                <input class="form-control ember-text-field" type="text" id="Amount" runat="server" />
                            </div>
                        </div>



                    </div>


                    <div id="tclayout" class="text-left" style="display: none;">
                        <div class="col-md-12" style="margin-top: 10px;">
                            <div class="row">
                                <label class="col-md-5">Table Border</label>
                                <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                    <input class="" type="checkbox" id="TableBorder1" runat="server" />
                                </div>
                                <div class="col-md-6" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="TableBorder" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=TableBorder.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <label class="control-label"><strong>Table Header</strong></label>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="control-label col-md-5">Font Size</label>
                                <div class="input-group col-md-7">
                                    <input class="form-control ember-text-field" type="text" id="headerFontSize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="col-md-5">Background Color</label>
                                <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                    <input class="" type="checkbox" id="tableheaderBackgroundColor1" runat="server" />
                                </div>
                                <div class="col-md-6" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="tableheaderBackgroundColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=tableheaderBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-5">Font Color</label>
                                <div class="col-md-7" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="tableheaderFontColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=tableheaderFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <label class="control-label"><strong>Item Row</strong></label>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="control-label col-md-5">Font Size</label>
                                <div class="input-group col-md-7">
                                    <input class="form-control ember-text-field" type="text" id="tableItemRowfontsize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="col-md-5">Background Color</label>
                                <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                    <input class="" type="checkbox" id="tableItemRowBackgroundColor1" runat="server" />
                                </div>
                                <div class="col-md-6" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="tableItemRowBackgroundColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=tableItemRowBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-5">Font Color</label>
                                <div class="col-md-7" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="tableItemRowFontColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=tableItemRowFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <label class="control-label"><strong>Item Description</strong></label>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="control-label col-md-5">Font Size</label>
                                <div class="input-group col-md-7">
                                    <input class="form-control ember-text-field" type="text" id="tableitemdescFontSize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-5">Font Color</label>
                                <div class="col-md-7" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input class="form-control" id="tableitemdescFontColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=tableitemdescFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                    <script type="text/javascript">

                        function showlabels() {

                            document.getElementById("tclabels").style.display = "block";
                            document.getElementById("tclayout").style.display = "none";


                            if ($("#btnlabels").hasClass("btn btn-default"))
                                $("#btnlabels").removeClass("btn btn-default").addClass("btn btn-primary");;
                            $("#btnlabels").addClass("btn btn-primary");


                            if ($("#btnLayout").hasClass("btn btn-primary"))
                                $("#btnLayout").removeClass("btn btn-primary").addClass("btn btn-default");;
                            $("#btnLayout").addClass("btn btn-default");


                        }

                        function showLayout() {
                            document.getElementById("tclayout").style.display = "block";
                            document.getElementById("tclabels").style.display = "none";



                            if ($("#btnLayout").hasClass("btn btn-default"))
                                $("#btnLayout").removeClass("btn btn-default").addClass("btn btn-primary");;
                            $("#btnLayout").addClass("btn btn-primary");


                            if ($("#btnlabels").hasClass("btn btn-primary"))
                                $("#btnlabels").removeClass("btn btn-primary").addClass("btn btn-default");;
                            $("#btnlabels").addClass("btn btn-default");


                        }
                    </script>









                    <script type="text/javascript">

                        function showtotallabels() {

                            document.getElementById("tclabelstotal").style.display = "block";
                            document.getElementById("tclayouttotal").style.display = "none";


                            if ($("#btnlabelstotal").hasClass("btn btn-default"))
                                $("#btnlabelstotal").removeClass("btn btn-default").addClass("btn btn-primary");;
                            $("#btnlabelstotal").addClass("btn btn-primary");


                            if ($("#btnLayouttotal").hasClass("btn btn-primary"))
                                $("#btnLayouttotal").removeClass("btn btn-primary").addClass("btn btn-default");;
                            $("#btnLayouttotal").addClass("btn btn-default");


                        }

                        function showtotalLayout() {
                            document.getElementById("tclayouttotal").style.display = "block";
                            document.getElementById("tclabelstotal").style.display = "none";



                            if ($("#btnLayouttotal").hasClass("btn btn-default"))
                                $("#btnLayouttotal").removeClass("btn btn-default").addClass("btn btn-primary");;
                            $("#btnLayouttotal").addClass("btn btn-primary");


                            if ($("#btnlabelstotal").hasClass("btn btn-primary"))
                                $("#btnlabelstotal").removeClass("btn btn-primary").addClass("btn btn-default");;
                            $("#btnlabelstotal").addClass("btn btn-default");


                        }
                    </script>







                </div>
                <div id="menu3" class="tab-pane fade">
                    <h4>Total</h4>

                    <div style="padding: 15px;">
                        <div class="btn-group">
                            <button type="button" id="btnlabelstotal" onclick="showtotallabels()" class="btn btn-primary">Labels</button>
                            <button type="button" id="btnLayouttotal" onclick="showtotalLayout()" class="btn btn-default">Layout</button>
                        </div>
                    </div>


                    <div id="tclabelstotal" style="display: block;">
                        <div class="form-group">
                            <div class="col-md-12">
                                <label>
                                    <input class="ember-checkbox" type="checkbox" id="ShowTotalSection" runat="server" />
                                    Show Total Section
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="SubTotal1" runat="server" />
                                        Sub Total
                                    </label>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <input class="form-control ember-text-field" type="text" id="SubTotal" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>Shipping Charges </label>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <input class="form-control ember-text-field" type="text" id="ShippingCharges" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>Total </label>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <input class="form-control ember-text-field" type="text" id="tableTotal" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>Currency Symbol </label>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <label>

                                    <asp:RadioButtonList ID="CurrencySymbol" runat="server" CssClass="radibuttonlistclass">
                                        <asp:ListItem>Before amount</asp:ListItem>
                                        <asp:ListItem>After amount</asp:ListItem>
                                    </asp:RadioButtonList>


                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="showitemQuantity" runat="server" />
                                        Quantity
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5">
                                <div>
                                    <label>Items in Total </label>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <input class="form-control ember-text-field" type="text" id="ItemsinTotal" runat="server" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <div>
                                    <label>
                                        <input class="ember-checkbox" type="checkbox" id="Showamountinwords" runat="server" />
                                        Show amount in words
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div id="tclayouttotal" class="text-left" style="display: none;">
                        <div class="col-md-12" style="margin-top: 10px;">
                            <label class="control-label"><strong>Total(Subtotal, Tax)</strong></label>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="control-label col-md-5">Font Size</label>
                                <div class="input-group col-md-7">
                                    <input class="form-control ember-text-field" type="text" id="TotalSubtotaltaxFontSize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-5">Font Color</label>
                                <div class="col-md-7" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input id="TotalSubtotaltaxFontColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=TotalSubtotaltaxFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <div class="row">
                                <label class="col-md-5">Background Color</label>
                                <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                    <input class="" type="checkbox" id="TotalSubtotaltaxBackgroundColor1" runat="server" />
                                </div>
                                <div class="col-md-6" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input id="TotalSubtotaltaxBackgroundColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=TotalSubtotaltaxBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <label class="control-label"><strong>Balance Due</strong></label>
                        </div>
                        <div class="col-md-12" style="margin-bottom: 10px;">
                            <div class="row">
                                <label class="control-label col-md-5">Font Size</label>
                                <div class="input-group col-md-7">
                                    <input class="form-control ember-text-field" type="text" id="BalanceDueFontSize" runat="server" />
                                    <span class="input-group-addon">pt</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <label class="col-md-5">Font Color</label>
                                <div class="col-md-7" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input id="BalanceDueFontColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=BalanceDueFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 10px;">
                            <div class="row">
                                <label class="col-md-5">Background Color</label>
                                <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                    <input class="" type="checkbox" id="BalanceDueBackgroundColor1" runat="server" />
                                </div>
                                <div class="col-md-6" style="padding-left: 0px;">
                                    <div class="input-group">
                                        <input id="BalanceDueBackgroundColor" runat="server" value="000000" />
                                        <span class="input-group-addon">
                                            <button class="jscolor {valueElement:'<%=BalanceDueBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div id="menu4" class="tab-pane fade">
                    <h4>Footer</h4>
                    <div class="col-md-12" style="margin-top: 15px;">
                        <label class="control-label"><strong>Notes</strong></label>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Label</label>
                            <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                <asp:CheckBox ID="chkisnotes" runat="server" />
                            </div>
                            <div class="col-md-6" style="padding-left: 0px;">
                                <input class="form-control ember-text-field" type="text" id="FooterNotesLabel" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Font Size</label>
                            <div class="input-group col-md-7">
                                <input class="form-control ember-text-field" type="text" id="FooterFontSize" runat="server" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12" style="margin: 0px 0 5px 0px;"><a href="/">Add your bank details</a></div>
                    <div class="col-md-12">
                        <label class="control-label"><strong>Terms & Conditions</strong></label>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Label</label>
                            <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">

                                <asp:CheckBox ID="chkistermsandcondition" runat="server" />
                            </div>
                            <div class="col-md-6" style="padding-left: 0px;">
                                <input class="form-control ember-text-field" type="text" id="TermsConditionsLabel" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Font Size</label>
                            <div class="input-group col-md-7">
                                <input class="form-control ember-text-field" type="text" id="TermsConditionsFontSize" runat="server" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <label class="control-label"><strong>Signature</strong></label>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="col-md-5">Label</label>
                            <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                <input class="" type="checkbox" id="isSignatur" runat="server" />
                            </div>
                            <div class="col-md-6" style="padding-left: 0px;">
                                <div class="input-group">
                                    <input class="form-control" type="text" value="Authorized Signature" id="isSignatureLabel" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-bottom: 5px;">
                        <label class="control-label">Signature image </label>
                        <div>
                            <span style="padding: 5px 12px; width: 100%;" bubbles="false" class="btn btn-default">
                                <input accept="image/gif,image/jpeg,image/png,image/bmp" class="upload" type="file">
                            </span>
                        </div>
                    </div>

                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Signature Name</label>
                            <div class="input-group col-md-7">
                                <input class="form-control ember-text-field" type="text" id="SignatureName" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <label class="control-label"><strong>Footer</strong></label>
                    </div>



                    <div class="col-md-12" style="margin-bottom: 10px;">
                        <div class="row">
                            <label class="control-label col-md-5">Font Size</label>
                            <div class="input-group col-md-7">
                                <input class="form-control ember-text-field" type="text" id="footersettingFontSize" runat="server" />
                                <span class="input-group-addon">pt</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="row">
                            <label class="col-md-5">Font Color</label>
                            <div class="col-md-7" style="padding-left: 0px;">
                                <div class="input-group">
                                    <input id="footersettingFontColor" runat="server" value="000000" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=footersettingFontColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-top: 10px;">
                        <div class="row">
                            <label class="col-md-5">Background Color</label>
                            <div class="col-md-1" style="padding-left: 0px; margin: 5px 0px;">
                                <input class="" type="checkbox" id="isfootersettingBackgroundColor" runat="server" />
                            </div>
                            <div class="col-md-6" style="padding-left: 0px;">
                                <div class="input-group">
                                    <input id="footersettingBackgroundColor" runat="server" value="000000" style="width: 100px" />
                                    <span class="input-group-addon">
                                        <button class="jscolor {valueElement:'<%=footersettingBackgroundColor.ClientID %>', onFineChange:'setTextColor(this)'}">&nbsp;</button>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <label class="control-label">Background Image </label>
                            <div>
                                <span style="padding: 5px 12px; width: 100%;" bubbles="false" class="btn btn-default">
                                    <input accept="image/gif,image/jpeg,image/png,image/bmp" class="upload" type="file">
                                </span>
                                <button type="button" bubbles="false" style="width: 100%; margin-top: 5px;" class="btn btn-default">Choose from Gallery</button>
                            </div>
                        </div>
                    </div>
                    <div class=" ">
                        <div class="col-md-12">
                            <label class="control-label">Image Position</label>
                            <div>
                                <asp:DropDownList ID="footersettingBackgroundImageposition" runat="server" CssClass="form-control">

                                    <asp:ListItem>Top left</asp:ListItem>
                                    <asp:ListItem>Top center</asp:ListItem>
                                    <asp:ListItem>Top right</asp:ListItem>
                                    <asp:ListItem>Center left</asp:ListItem>
                                    <asp:ListItem>Center center</asp:ListItem>
                                    <asp:ListItem>Center right</asp:ListItem>
                                    <asp:ListItem>Bottom left</asp:ListItem>
                                    <asp:ListItem>Bottom center</asp:ListItem>
                                    <asp:ListItem>Bottom right</asp:ListItem>

                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <label>
                            <input class="ember-checkbox" type="checkbox" id="showpageno" runat="server" />
                            Show Page Number
                        </label>
                    </div>
                    <div class="col-md-12" style="margin: 10px 0 5px 0px;"><a href="/">Customize your footer content</a></div>

                </div>

            </div>
            <div class="list-header topgrad">
                <div class="list-filter">

                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-primary" />

                    <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="btn btn-default" />


                    <%--  <button type="submit" class="btn btn-primary"> Save </button>
                            <button class="btn btn-default">Preview</button>--%>
                </div>
            </div>

        </div>

        <div class="col-md-12" style="padding-left: 0px; padding-right: 0px;background:#fff;">

          
             <asp:Panel ID="pnlhistory" runat="server" >  
            <div class="grey-bg displaynonexs">
                <div class="comments-history">

                    <a id="loadMore" class="btn btn-success btn-sm"><%=fns.getltext("+ View more", "2")%></a>
                    <a id="showLess" class="btn btn-danger btn-sm"><%=fns.getltext("- View less", "2")%></a>
                    <ul class="list-unstyled" id="myList">

                        <%=viewhistorystring %>
                    </ul>
                </div>
            </div>
                 </asp:Panel>
            <script>
                $(document).ready(function () {
                    size_li = $("#myList li").size();
                    x = 3;
                    $('#myList li:lt(' + x + ')').show();
                    $('#loadMore').click(function () {
                        x = (x + 5 <= size_li) ? x + 5 : size_li;
                        $('#myList li:lt(' + x + ')').show();
                    });
                    $('#showLess').click(function () {
                        x = (x - 5 < 0) ? 3 : x - 5;
                        $('#myList li').not(':lt(' + x + ')').hide();
                    });
                });
            </script>
            <div class="col-md-12">
            <div class="pcs-template-body" style="box-shadow: 0px 0px 5px #ccc; padding: 30px; margin: 20px 0px 20px 0px; float: left;background:#fff" id="invoice">
                <div class="table-responsive">
                    <%=prvdata %>
                </div>

            </div>
            </div>

        </div>






        <div class="modal fade in inner-modal" id="viewpdf" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog" style="width: 100%; margin: 0px;">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="hidemaster()"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
                            <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Preview", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="viewpdf" runat="server"></asp:Label>
                    </div>

                </div>
            </div>
        </div>



        <div class="modal fade in inner-modal" id="viewpdfonletterhead" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog" style="width: 100%; margin: 0px;">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="hidemaster()"><span>x</span> </button>
                    <div id="Div2">
                        <header id="Header2" class="navbar navbar-blue navbar-fixed-top" role="banner">
                            <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Preview", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="viewpdfonletterhead" runat="server"></asp:Label>
                    </div>

                </div>
            </div>
        </div>



        <div class="modal fade in inner-modal" id="myModalassets" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
                            <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Expected Delivery Date", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <table class="table table-striped table-bordered datatables">
                            <tr>
                                <td>

                                    <%=fns.getltext("Date", "2")%>
                                </td>

                                <td>
                                    <uc1:uc1 ID="EstimateDate" runat="server" requeredDisplay="Dynamic" requeredEnabled="true" ValidationGroup="dsave" />
                                </td>
                            </tr>
                            <tr>
                                <td><%=fns.getltext("Notes", "2")%></td>
                                <td>
                                    <asp:TextBox ID="txtnotes" runat="server" TextMode="MultiLine"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td>

                                    <asp:Button ID="btndeliverydatesave" runat="server" Text="Save" OnClick="btndeliverydatesave_click" ValidationGroup="dsave" />
                                </td>
                            </tr>

                        </table>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal fade in inner-modal" id="canceldiv" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Cancel Sales Order", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <table class="table table-striped table-bordered datatables">
                            <tr>

                                <td><b><%=fns.getltext("Once you cancel a Sales Order, it cannot be undone. Are you sure you want to continue?", "2")%></b> </td>
                            </tr>

                            <tr>
                                <td>
                                    <%=fns.getltext("Remarks", "2")%>
                                    <asp:TextBox ID="txtcancelremarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="gicancel" Text="*" 
           ControlToValidate="txtcancelremarks" runat="server" ErrorMessage="Please Enter Remarks !!!"></asp:RequiredFieldValidator>--%>



          
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=fns.getltext("Reasons", "2")%>
                                    <asp:UpdatePanel ID="updaapp44" runat="server">
                                        <ContentTemplate>
                                            <cc1:DropDownListChosen ID="ddlreasonscancel" runat="server" NoResultsText="No results match." Width="300px"
                                                DataPlaceHolder="Type Here..." AllowSingleDeselect="true" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlreasonscancel_change">
                                            </cc1:DropDownListChosen>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="gicancel" SetFocusOnError="true" InitialValue="0"
                                                Text="*" ControlToValidate="ddlreasonscancel" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtotherresonecancel" runat="server" Visible="false" placeholder="Enter Reason" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="gicancel" SetFocusOnError="true"
                                                Text="*" ControlToValidate="txtotherresonecancel" Enabled="false" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>

                                        <asp:Button CssClass="btn btn-danger" ID="btncancelyes" runat="server" Text="Yes" OnClick="btncancelyes_click" Width="100px" ValidationGroup="gicancel" />
                                        <asp:Button CssClass="btn btn-primary" ID="btncancelNo" runat="server" Text="No" OnClick="btncancelNo_click" Width="100px" /></center>
                                </td>
                            </tr>

                        </table>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal fade in inner-modal" id="lockdiv" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Lock Sales Order", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <br />
                        <table class="table table-striped table-bordered datatables">
                            <tr>

                                <td><b><%=fns.getltext("Are you sure want to lock the sales order?", "2")%></b> </td>
                            </tr>

                            <tr>
                                <td>
                                    <center>

                                        <asp:Button CssClass="btn btn-danger" ID="Button2" runat="server" Text="Yes" OnClick="btncancelyes1_click" Width="100px" />
                                        <asp:Button CssClass="btn btn-primary" ID="Button4" runat="server" Text="No" OnClick="btncancelNo1_click" Width="100px" /></center>
                                </td>
                            </tr>

                        </table>
                    </div>

                </div>
            </div>
        </div>
        <div class="modal fade in inner-modal" id="canceldivitem" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog" style="width: 750px !important;">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Cancel Sales Order Items", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <table class="table table-striped table-bordered datatables">
                            <tr>

                                <td><b>Once you cancel a Sales Order Items, it cannot be undone. Are you sure you want to continue?</b> </td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:HiddenField ID="hdncount" runat="server" />
                                    <asp:GridView ID="Gridview1" runat="server" CssClass="table table-striped table-bordered table-hover"
                                        AutoGenerateColumns="false">
                                        <Columns>


                                            <asp:TemplateField HeaderText="Item Details">
                                                <ItemTemplate>
                                                    <%# Eval("pname") %> <%#showparm(Eval("prm1").ToString(),Eval("prm2").ToString(),Eval("prm3").ToString(),Eval("prm4").ToString(),Eval("prm5").ToString(),Eval("prm6").ToString(),Eval("prm7").ToString(),Eval("prm8").ToString(),Eval("prm9").ToString()) %>
                                                    <asp:HiddenField ID="hdnestmid" runat="server" Value='<%#Eval("estmid")%>' />
                                                    <asp:HiddenField ID="hdnitemid" runat="server" Value='<%#Eval("itemid")%>' />
                                                    <asp:HiddenField ID="hdnprm1" runat="server" Value='<%#Eval("prm1")%>' />
                                                    <asp:HiddenField ID="hdnprm2" runat="server" Value='<%#Eval("prm2")%>' />
                                                    <asp:HiddenField ID="hdnprm3" runat="server" Value='<%#Eval("prm3")%>' />
                                                    <asp:HiddenField ID="hdnprm4" runat="server" Value='<%#Eval("prm4")%>' />
                                                    <asp:HiddenField ID="hdnprm5" runat="server" Value='<%#Eval("prm5")%>' />
                                                    <asp:HiddenField ID="hdnprm6" runat="server" Value='<%#Eval("prm6")%>' />
                                                    <asp:HiddenField ID="hdnprm7" runat="server" Value='<%#Eval("prm7")%>' />
                                                    <asp:HiddenField ID="hdnprm8" runat="server" Value='<%#Eval("prm8")%>' />
                                                    <asp:HiddenField ID="hdnprm9" runat="server" Value='<%#Eval("prm9")%>' />
                                                    <asp:HiddenField ID="hdnrate" runat="server" Value='<%#Eval("rate")%>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <%# Eval("rate") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Quantity">
                                                <ItemTemplate>
                                                    <%# Eval("quantity") %>
                                                    <br />
                                                    <%# Eval("lblunit") %>
                                                    <asp:HiddenField ID="hdnpoqty" runat="server" Value='<%# Eval("quantity") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Received Quantity">
                                                <ItemTemplate>
                                                    <%#showreceiveqty(Eval("estmid").ToString(),Eval("itemid").ToString(),Eval("rate").ToString(),Eval("prm1").ToString(),Eval("prm2").ToString(),Eval("prm3").ToString(),Eval("prm4").ToString(),Eval("prm5").ToString(),Eval("prm6").ToString(),Eval("prm7").ToString(),Eval("prm8").ToString(),Eval("prm9").ToString()) %>

                                                    <asp:HiddenField ID="hdnrcvqty" runat="server" Value='<%#showreceiveqty(Eval("estmid").ToString(),Eval("itemid").ToString(),Eval("rate").ToString(),Eval("prm1").ToString(),Eval("prm2").ToString(),Eval("prm3").ToString(),Eval("prm4").ToString(),Eval("prm5").ToString(),Eval("prm6").ToString(),Eval("prm7").ToString(),Eval("prm8").ToString(),Eval("prm9").ToString()) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Canceled Quantity">
                                                <ItemTemplate>
                                                    <%#showcanecelqty(Eval("estmid").ToString(),Eval("itemid").ToString(),Eval("rate").ToString(),Eval("prm1").ToString(),Eval("prm2").ToString(),Eval("prm3").ToString(),Eval("prm4").ToString(),Eval("prm5").ToString(),Eval("prm6").ToString(),Eval("prm7").ToString(),Eval("prm8").ToString(),Eval("prm9").ToString()) %>

                                                    <asp:HiddenField ID="hdncancelqty" runat="server" Value='<%#showcanecelqty(Eval("estmid").ToString(),Eval("itemid").ToString(),Eval("rate").ToString(),Eval("prm1").ToString(),Eval("prm2").ToString(),Eval("prm3").ToString(),Eval("prm4").ToString(),Eval("prm5").ToString(),Eval("prm6").ToString(),Eval("prm7").ToString(),Eval("prm8").ToString(),Eval("prm9").ToString()) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cancel Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcancelqty" runat="server" onkeyup="calculate()" AutoComplete="off" onkeypress="return isnumeric(event)"></asp:TextBox>
                                                    <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Enabled="false"></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cancel Reason">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcancelresone" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                    <script type="text/javascript">
                                        function calculate() {

                                            var rcvqty = 0;
                                            var cancelqty = 0;
                                            var remqty = 0;
                                            var count = $("#ContentPlaceHolder1_hdncount").val();
                                            console.log("count=" + count);
                                            for (var x = 0; x < count; x++) {
                                                var j = x;


                                                var val1 = $("#ContentPlaceHolder1_Gridview1_hdnpoqty_" + j).val();
                                                var val2 = $("#ContentPlaceHolder1_Gridview1_hdnrcvqty_" + j).val();
                                                var val3 = $("#ContentPlaceHolder1_Gridview1_hdncancelqty_" + j).val();
                                                var val5 = $("#ContentPlaceHolder1_Gridview1_txtcancelqty_" + j).val();

                                                var val4 = parseFloat(val1) - (parseFloat(val2) + parseFloat(val3));
                                                if (parseFloat(val5) > parseFloat(val4)) {

                                                    alert("Cancel Quantiy is not be greater than Remaining Qty " + val4 + "");
                                                    $("#ContentPlaceHolder1_Gridview1_txtcancelqty_" + j).val() = "";
                                                }

                                            }

                                        }
                                    </script>

                                </td>
                            </tr>


                            <tr>
                                <td>
                                    <center>

                                        <asp:Button CssClass="btn btn-danger" ID="btncancelitemdetails" runat="server" Text="Yes" OnClientClick="return confirm('Are you sure to cancel items?')" OnClick="btncancelitemdetails_click" Width="100px" />
                                        <asp:Button CssClass="btn btn-primary" ID="Button3" runat="server" Text="No" OnClick="btncancelNo_click" Width="100px" /></center>
                                </td>
                            </tr>

                        </table>
                    </div>

                </div>
            </div>
        </div>

        <div class="modal fade in inner-modal" id="deletediv" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
            <div class="modal-dialog">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><span>x</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
          <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("Delete Sales Order", "2")%></h5>
          </div>
        </header>
                        <br />
                        <br />
                        <br />
                        <br />
                        <table class="table table-striped table-bordered datatables">
                            <tr>

                                <td><b><%=fns.getltext("Sales Order will be deleted forever and cannot be retrieved later. Are you sure about deleting it?", "2")%></b> </td>
                            </tr>

                            <tr>
                                <td>
                                    <%=fns.getltext("Remarks", "2")%>
                                    <asp:TextBox ID="txtdeleteremarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="gidelete" Text="*"
                                        ControlToValidate="txtdeleteremarks" runat="server" ErrorMessage="Please Enter Remarks !!!"></asp:RequiredFieldValidator>




                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <center>

                                        <asp:Button CssClass="btn btn-danger" ID="btndeletepurchase" runat="server" Text="deleting it" OnClick="btndeletepurchase_click" Width="100px" ValidationGroup="gidelete" />
                                        <asp:Button CssClass="btn btn-primary" ID="btndeletepurchasecancel" runat="server" Text="No" OnClick="btndeletepurchasecancel_click" Width="100px" /></center>
                                </td>
                            </tr>

                        </table>
                    </div>

                </div>
            </div>
        </div>
        <div runat="server" class="modal fade in inner-modal" id="viewquantity" role="dialog" aria-labelledby="myModalLabel" style="display: none;">
            <div class="modal-dialog" style="width: 900px; margin-top: 5%;">
                <div class="modal-content">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="showComponentHide()"><span>x&nbsp;&nbsp;&nbsp;</span> </button>
                    <div id="wrapper">
                        <header id="topnav" class="navbar navbar-blue navbar-fixed-top" role="banner">
             <div class="title-bar" style="height:46px;padding:15px;">
            <h5 style="margin:0;"><%=fns.getltext("View BOM", "2")%></h5>
          </div>
        </header>
                        <iframe id="viewquantitysrc" src="" width="100%" height="500"></iframe>

                    </div>

                </div>
            </div>

        </div>
        <script type="text/javascript">
            function openqty(val1) {



                document.getElementById("<%=viewquantity.ClientID%>").style.display = "block";
                document.getElementById("viewquantitysrc").src = "View_Quantity_popupbom.aspx?id=" + val1;

            }

            function showComponentHide(val) {

                document.getElementById("<%=viewquantity.ClientID%>").style.display = "none";
            }

        </script>


        <asp:Panel ID="pnlapprove2" runat="server">
            <h2><%=fns.getltext("Approve/Rejection", "2")%></h2>
            <table class="table table-striped table-bordered">

                <tr>
                    <td>
                        <b><%=fns.getltext("Remarks", "2")%></b></td>

                    <td>
                        <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="50%"></asp:TextBox><br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td><b><%=fns.getltext("Status", "2")%></b>



                    </td>
                    <td>


                        <asp:UpdatePanel ID="updaapp" runat="server">
                            <ContentTemplate>



                                <cc1:DropDownListChosen ID="ddlastatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlastatus_change" Width="300px">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Approve</asp:ListItem>
                                    <asp:ListItem Value="2">Reject</asp:ListItem>
                                </cc1:DropDownListChosen>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="gi" SetFocusOnError="true" InitialValue="0"
                                    Text="*" ControlToValidate="ddlastatus" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                                <asp:Panel ID="pnlreasone" runat="server" Visible="false">
                                    <b><%=fns.getltext("Reasons", "2")%></b>&nbsp;
                                        <cc1:DropDownListChosen ID="ddlreasone" runat="server" NoResultsText="No results match." Width="300px"
                                            DataPlaceHolder="Type Here..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ddlreasone_change">
                                        </cc1:DropDownListChosen>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="gi" SetFocusOnError="true" InitialValue="0"
                                        Text="*" ControlToValidate="ddlreasone" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtotherresone" runat="server" Visible="false" placeholder="Enter Reason" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="gi" SetFocusOnError="true"
                                        Text="*" ControlToValidate="txtotherresone" Enabled="false" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>

                <tr>

                    <td colspan="2">
                        <center>

                            <div id="pnlimg" style="display: none;">

                                <img src="/images/loading8.gif" runat="server" id="imgload" />


                            </div>
                            <div id="divsub" style="display: block;">

                                <asp:Button ID="btnsaveapp" OnClientClick="return showdivimg()" Width="100px" ValidationGroup="gi" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnsaveapp_click" />&nbsp;
                            </div>

                            


                        </center>

                        <div style="display: none;">

                            <asp:Button ID="btnapprove" runat="server" CssClass="btn btn-success" Text="Approve" OnClick="btnapprove_click" />&nbsp;
                        <asp:Button ID="btnreject" runat="server" CssClass="btn btn-danger" OnClientClick=" return confirm('Are you sure you want to reject this entry?')" Text="Reject" OnClick="btnreject_click" />
                        </div>
                    </td>
                </tr>
            </table>



            <br />
            <br />
            <%--   <center>
                      <b> Remarks</b>&nbsp; <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="50%" ></asp:TextBox><br /><br />
                    <asp:Button ID="btnapprove" runat="server" CssClass="btn btn-success" Text="Approve"  OnClick="btnapprove_click" />&nbsp;
                        <asp:Button ID="btnreject" runat="server" CssClass="btn btn-danger" OnClientClick=" return confirm('Are you sure you want to reject this entry?')" Text="Reject"  OnClick="btnreject_click" />
                    </center>
            <br />  <br />--%>
            

        </asp:Panel >
         <script type="text/javascript">
         //$(document).ready(function !IsPostBack() {
         //    debugger
         //    refreshdata();
         //});
        debugger
          var count = 1;
        function loadsrcedit_saleorder(val1, val2, val3) {
            debugger
            count++;
            try {
                document.getElementById("dv_viewsale").style.display = 'none';
                 document.getElementById("dvmangesale").style.display = 'none';
               
            }
            catch (ex) {
            }
            document.getElementById(val1).src = "";
            if (val2.includes('?')) {
                      document.getElementById(val1).src = val2 + "&count=" + count;
                 }
                 else {
                      document.getElementById(val1).src = val2 + "?count=" + count;
                 }
            
           
        }

      

        

    </script>
   
  
 <div class="modal fade in inner-modal" id="editsaleorder" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
  <div class="modal-dialog modal-fullscreen" style="width:100%;margin:0px;height:100%;" >
    <div class="modal-content"  style="width:100%;height:100%;">
      
      <div id="wrapper" style="width:100%;  height:100%;" class="modal-content">
          
              <button type="button" id="closenewsaleedit" class="close" style="display:none" data-dismiss="modal" aria-hidden="true"> <span>x</span> </button>
          

            
        <iframe id='myModalsaleorderedit' width="100%" height="100%" style="margin-top:0px;padding:0px 0px;border:0;" border="0px"></iframe>
           

          
            </div>

            </div>
            </div>
            </div>  
</asp:Content>

