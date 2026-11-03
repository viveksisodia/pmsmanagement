<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="MultiProjectTask.aspx.cs" Inherits="MultiProjectTask" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/MultipleData.ascx" TagPrefix="uc1" TagName="MultipleData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <style>
.dataTables_scrollBody {min-height:300px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content" style="padding-bottom:0px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <uc1:MultipleData runat="server" ID="MultipleData" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
<script>  
       
$(function() {
  $('#showtr').on('click', function() {
    $('.taskshow').toggle();
  });
});
$('#showtr').click(function() {
    $(this).toggleClass('shown');
        });

 //$(function() {
 //    $('#showtrM_14').on('click', function () {
 //        alert('ok') 
 //        $('.taskshow_14').toggle();
 //    });
 //});
 //       $('#showtrM_14').click(function () {
 //           alert('ok1');
 //           $(this).toggleClass('shown');
 //       });



</script>
   
<script>
    
$(function() {
  $('#showtr2').on('click', function() {
    $('.taskshow2').toggle();
  });
});
$('#showtr2').click(function() {
    $(this).toggleClass('shown');
    });

    //$(function () {
    //    $('#showtrM_14').on('click', function ()
    //    {
    //        alert('ok')
    //        $('.taskshow_14').toggle();
    //    });
    //});
    //$('#showtrM_14').click(function () {
    //    alert('ok1')
    //    $(this).toggleClass('shown');
    //});

   // $(function () { $('#showtasktr_35').on('click', function () { $('.subtaskshow_35').toggle(); }); }); $('#showtasktr_35').click(function () { debugger  $(this).toggleClass('shown'); }); $(function () { $('#showtasktr_36').on('click', function () { $('.subtaskshow_36').toggle(); }); }); $('#showtasktr_36').click(function () { debugger  $(this).toggleClass('shown'); }); $(function () { $('#showtrM_7040').on('click', function () { alert('ok') $('.taskshow_7040').toggle(); }); }); $('#showtrM_7040').click(function () { alert('ok1') $(this).toggleClass('shown'); }); $(function () { $('#showtasktr_2089').on('click', function () { $('.subtaskshow_2089').toggle(); }); }); $('#showtasktr_2089').click(function () { debugger  $(this).toggleClass('shown'); });

</script>

     
</asp:Content>
