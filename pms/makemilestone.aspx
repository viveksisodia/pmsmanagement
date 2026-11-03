<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="makemilestone.aspx.cs" Inherits="makemilestone" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/setdurationtime.ascx" TagPrefix="uc1" TagName="setdurationtime" %>



<asp:Content ID="Content3" ContentPlaceHolderID="head" Runat="Server">
   
      <style>
/*.dataTables_scrollBody {min-height:250px;}*/
.dataTables_scrollBody {height: calc(100vh - 200px);}
.dropdown-menu.dropdownmenu-secondary.show {text-align:center;}
.preview-item:hover {transform: scale(1.25);}
.preview-item img {width: 30px;height: 30px;border-radius: 50%;}
.owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
a.owner-wrp {position:relative;}
a.owner-wrp:hover .owner-del {display:block;}
.files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}

</style>
   <!-- OUTSIDE of UpdatePanel -->
   
     <div style="display: none;">
          <a href = 'javascript:void(0);' id="btnreload1" style="display:none" onclick="reloaddata()"></a>
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnreload" onclick="Bindtask_data();" aria-label="Save">reload</a>
                        </div>

   
   <script>
       try {
    //debugger;

    // Dispose existing tooltips
    const oldTooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    oldTooltips.forEach(function (el) {
        const tooltip = bootstrap.Tooltip.getInstance(el);
        if (tooltip) {
            tooltip.dispose();
        }
    });

    // Select elements with data-bs-toggle and update attributes
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(function (el) {
        el.setAttribute('data-bs-placement', 'top');
        el.setAttribute('data-bs-trigger', 'hover');
    });

    // Initialize new tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl, {
            placement: 'top',
            trigger: 'hover'
        });
    });

} catch (e) {
    console.error('Tooltip init error:', e);
}
   </script>

   <script>
function selectapply(ctrlid)
{
debugger
//alert(ctrlid);
var lblid=ctrlid.split("_")[1];
const selectedSpan = document.querySelector('#lblselected_'+lblid);
    if (selectedSpan) {
       var selectedValue = selectedSpan.textContent.trim().split('-'); 

       var selectedfdate= selectedValue[0].trim();
       var selectedtdate= selectedValue[1].trim();
      //var formattedDate_from = formatDate(selectedfdate);
      //var formattedDate_to = formatDate(selectedtdate);
      var strdateFV=formatDateNew(selectedfdate);
      var strdateTV=formatDateNew(selectedtdate);
      var daysno=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate) ,'ndays');
      var days=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate),'days');
       var strdays="";
if(days<0)
{
strdays= -((days+1))+" days delayed";
}
else if((days-1)>0)
{
strdays= days+" days left";
}
 let isValid = true;
        //alert(formatDateNew(formattedDate_from)+"-"+formatDateNew(formattedDate_to));
        var ids=document.getElementById('<%=hiddenDatePicker.ClientID%>').value.split("_");
        if(ids.length>0)
        {
            if(ids[3]!=""&&ids[3]!="0")
            {
            isValid= checkdatecomparisiontaskdate_subtask(selectedValue[0].trim(),selectedValue[1].trim(),ids[2]);
            }
           else if(ids[2]!=""&&ids[2]!="0")
            {
           isValid= checkdatecomparisiontaskdate_task(selectedValue[0].trim(),selectedValue[1].trim(),ids[1]);
            }
          else if(ids[1]!=""&&ids[1]!="0")
            {
           isValid= checkdatecomparisiontaskdate_mile2(selectedValue[0].trim(),selectedValue[1].trim(),ids[0]);
            }
if(isValid)
{
debugger
updatetimeline(ids[0],ids[1],ids[2],ids[3],selectedValue[0].trim(),selectedValue[1].trim());
    if(ids[3]!=""&&ids[3]!="0")
    {

      const strtitle = document.querySelector('#title1_'+ids[0]+"_"+ids[1]+"_"+ids[2]+"_"+ids[3]);
        strtitle.title = strdays;
        strtitle.setAttribute('data-bs-original-title', strdays);
    const strspan1 = document.querySelector('#spn1_'+ids[0]+"_"+ids[1]+"_"+ids[2]+"_"+ids[3]);
    strspan1.textContent=strdateFV+"-"+strdateTV;
    const strspan2 = document.querySelector('#spn2_'+ids[0]+"_"+ids[1]+"_"+ids[2]+"_"+ids[3]);
    strspan2.textContent=daysno+" days";
    }
    else if(ids[2]!=""&&ids[2]!="0")
    {
    const strtitle = document.querySelector('#title1_'+ids[0]+"_"+ids[1]+"_"+ids[2]);
        strtitle.title = strdays;
        strtitle.setAttribute('data-bs-original-title', strdays);
    const strspan1 = document.querySelector('#spn1_'+ids[0]+"_"+ids[1]+"_"+ids[2]);
    strspan1.textContent=strdateFV+"-"+strdateTV;
    const strspan2 = document.querySelector('#spn2_'+ids[0]+"_"+ids[1]+"_"+ids[2]);
    strspan2.textContent=daysno+" days";
    }
    else if(ids[1]!=""&&ids[1]!="0")
    {
    const strtitle = document.querySelector('#title1_'+ids[0]+"_"+ids[1]);
        strtitle.title = strdays;
        strtitle.setAttribute('data-bs-original-title', strdays);
    const strspan1 = document.querySelector('#spn1_'+ids[0]+"_"+ids[1]);
    strspan1.textContent=strdateFV+"-"+strdateTV;
    const strspan2 = document.querySelector('#spn2_'+ids[0]+"_"+ids[1]);
    strspan2.textContent=daysno+" days";
    }
}
else
{
return false;
}


}



}



}
function formatDateNew(dateInput) {
//const date = new Date(dateInput);
var dt=dateInput.split("/");
 var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
  //var dd = String(date.getDate()).padStart(2, '0');
  var dd = dt[1];
  //var MMM = months[date.getMonth()];
  var MMM = months[parseInt(dt[0])-1];
  //var yyyy = date.getFullYear();
  var yyyy = dt[2];
  return dd + ' ' + MMM + ' ' + yyyy;
}
function formatDateDiff(dateInput1, dateInput2,strtype) {
  const date1 = new Date(dateInput1);
  const date2 = new Date(dateInput2);
  const date3 = new Date();
if(strtype=="ndays")
{
      const diffTime = Math.abs(date2 - date1);
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      return diffDays;
}
else
{
    if(date2<date3)
    {
      const diffTime = Math.abs(date3 - date2);
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      return 0-diffDays;
    }
    else
    {
    const diffTime = Math.abs(date3 - date2);
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      return diffDays;
    }
    }


  
}

function generateids(rowIndex,strpid,strmid,strtid,strstid,strtype)
{
//debugger
var strval=strpid+"_"+strmid+"_"+(strtid==""?"0":strtid)+"_"+(strstid==""?"0":strstid)
document.getElementById('<%=hiddenDatePicker.ClientID%>').value=strval;
var divs = document.querySelectorAll('.drp-selected');
divs.forEach(function(div, index) {
  div.id = 'lblselected_' + index; // e.g., yourNewId0, yourNewId1, etc.

});

var divs = document.querySelectorAll('.applyBtn');
divs.forEach(function(div, index) {
  div.id = 'btnapply_' + index; // e.g., yourNewId0, yourNewId1, etc.
if (index === (parseInt(rowIndex)-1)) {
      // Properly format onclick string with quotes around string parameters
      div.setAttribute('onclick', 'return selectapply(this.id)');
    } else {
      // Use empty strings as parameters
      div.setAttribute('onclick', `return selectapply(this.id)`);
    }
});


}

function addeventdateselect(strpid,strmid,strtid,strstid,strtype,btn)
{

//debugger
 
        const table =document.querySelector('#myTable'); 
        const row = btn.closest('tr');
        const rowIndex = Array.from(table.rows).indexOf(row);
        generateids(rowIndex,strpid,strmid,strtid,strstid,strtype);

        // const visibleRows = Array.from(row.parentElement.querySelectorAll('tr'))
        //.filter(r => getComputedStyle(r).display !== 'none');
   // const rowIndex = visibleRows.indexOf(row);

 //const applyBtn = targetRow.querySelector('.applyBtn');
 const applyBtn = table.rows[rowIndex].querySelector('.timesheet-wrap .applyBtn');
if (applyBtn) {
 applyBtn.addEventListener('click', function () {
//debugger
  const selectedSpan = document.querySelector('.drp-selected');
    if (selectedSpan) {
       var selectedValue = selectedSpan.textContent.trim().split('-'); 

       var selectedfdate= makedatepart(selectedValue[0].trim());
       var selectedtdate= makedatepart(selectedValue[1].trim());
      var formattedDate_from = formatDate(selectedfdate);
      var formattedDate_to = formatDate(selectedtdate);

       
      alert(formattedDate_from+"-"+formattedDate_to);
}
});
}

 }

function makedatepart(selectedValue)
{
//debugger
 // Parse the date string into components
    var parts = selectedValue.split('/'); // [MM, DD, YYYY]
    var month = parseInt(parts[0], 10) - 1; // JS months are 0-based
    var day = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);

    // Create Date object
    var selectedDay = new Date(year, month, day);
    return selectedDay;
}
function formatDate(date) {
  var mm = String(date.getMonth() + 1).padStart(2, '0');
  var dd = String(date.getDate()).padStart(2, '0');
  var yyyy = date.getFullYear();
  return mm + '/' + dd + '/' + yyyy;
}

function viewdateformat(dateStr) {
    let d = new Date(dateStr);

    let day = d.getDate(); // no leading zero
    let month = d.toLocaleString('en-GB', { month: 'short' }); // Jul, Aug
    let year = d.getFullYear();

    return `${day} ${month} ${year}`;
}


   </script> 
    <script>
function viewdateformatnew(dateStr) {
    let d = new Date(dateStr);

    let day = d.getDate(); // no leading zero
    //let month = d.toLocaleString('en-GB', { month: 'short' }); // Jul, Aug
let month = String(d.getMonth() + 1).padStart(2, '0'); // 01-12
    let year = d.getFullYear();

    return `${day}/${month}/${year}`;
}


    </script>
    <script>
       

         function updatetimeline(strpid,strmid,strtid,strstid,strfromdate,strtodate) {
                //debugger      
        
 $.ajax({
                            type: "POST",
                            url: "makemilestone.aspx/updatetimeline",                            
                            data: "{'pid':'"+strpid+"','mid':'"+strmid+"','tid':'"+strtid+"','stid':'"+strstid+"','fromdate':'"+strfromdate+"','todate':'"+strtodate+"'}",
                          contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                    //debugger                                   
                                    if(response.d=="1")
                                    {
                                    //alert("Update Successfully");                                     
                                    }                                       

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            
                                    });
         }
    </script>
    
    <script type="text/javascript">
        function addviewreset() {
Bindtask_data();
viewalldetails();
        }
    function toggleHiddenFieldValue(strpid, strmid, strtid,strtype) {
        //debugger;
        var value = strpid;

        if (strmid !== "") {
            value += "_" + strmid;
        }

        if (strtid !== "") {
            value += "_" + strtid;
        }

        var hiddenField = document.getElementById('<%= hdnM.ClientID %>');
        var current = hiddenField.value;

        var values = current ? current.split(',') : [];
        var index = values.indexOf(value);

        if (index === -1) {
            // Add value
            if(strtype!="delete")
            {
             values.push(value);
            }
        } else {
            // Remove value
if(strtype!="add")
            {
            var chkar = value.split('_');
            if (chkar.length == 2) {

                const results = values.filter(item => item.startsWith(value + '_'));
                if (results.length > 0) {
                    for (var j = 0; j < results.length; j++) {
                        var index1 = values.indexOf(results[j]);
                        if (index1 !== -1) {
                            values.splice(index1, 1);
                            var res = results[j].split('_');
                            var taskElem = document.getElementById('showtasktr_' + res[2]);
                                if (taskElem) {
                                    taskElem.click();
                                }
                       }
                    }

                }
            }
            values.splice(index, 1);
}
        }

        hiddenField.value = values.join(',');

    }
</script>
    
    <script>
function viewalldetails()
{
//debugger
hiddenField =document.getElementById('<%=hdnM.ClientID %>');
 var current=hiddenField.value;
var values = current?current.split(','):[];
    if(values.length>0)
    {
        for(var i=0;i<values.length;i++)
        {
            var pmt=values[i].split('_');
            if(pmt.length==2)
            {
            viewtask1(pmt[0],pmt[1]);
            }
            else if(pmt.length==3)
            {
            viewsubtask1(pmt[0],pmt[1],pmt[2]);
            }
        }
    }
$('.timesheet-wrap').daterangepicker({
    opens: 'center',
  });

$('.dayswrapper').daterangepicker({
    opens: 'center',
    locale: {
      format: 'MM/DD/YYYY'
    }
  });
       
 }

        try {
    //debugger;

    // Dispose existing tooltips
    const oldTooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    oldTooltips.forEach(function (el) {
        const tooltip = bootstrap.Tooltip.getInstance(el);
        if (tooltip) {
            tooltip.dispose();
        }
    });

    // Select elements with data-bs-toggle and update attributes
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(function (el) {
        el.setAttribute('data-bs-placement', 'top');
        el.setAttribute('data-bs-trigger', 'hover');
    });

    // Initialize new tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl, {
            placement: 'top',
            trigger: 'hover'
        });
    });

} catch (e) {
    console.error('Tooltip init error:', e);
}

    </script>
    <script>

function editmdays(strlblid)
{
//debugger
var txtid='txtday_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}
function edittaskdays(strlblid)
{
//debugger
var txtid='txtday_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3]+'_'+strlblid.split('_')[4];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}
function editsubtaskdays(strlblid)
{
//debugger
var txtid='txtday_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3]+'_'+strlblid.split('_')[4]+'_'+strlblid.split('_')[5];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}


function edittext(strlblid)
{
//debugger
var txtid='txtm_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}

function edittexttask(strlblid)
{
//debugger
var txtid='txtm_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3]+'_'+strlblid.split('_')[4];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}

function edittextsubtask(strlblid)
{
//debugger
var txtid='txtm_'+strlblid.split('_')[1]+'_'+strlblid.split('_')[2]+'_'+strlblid.split('_')[3]+'_'+strlblid.split('_')[4]+'_'+strlblid.split('_')[5];
document.getElementById(strlblid).style.display='none';
document.getElementById(txtid).style.display='';
document.getElementById(txtid).focus();
}
function updatedetails(txtid,recordId,columname) {
//debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value.replace(/\s+$/, "");

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblm_"+parts[1]+"_"+parts[2]+"_"+parts[3]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdateHandler.aspx', {
        method: 'POST',
        headers: {

            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: recordId, columnName: columnName, updateText: updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
            txt.style.display = 'none';
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
//lbl.style.minWidth='20px';
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
}
function updatedetails_hr(txtid,recordId,columname) {
//debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value.replace(/\s+$/, "");

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblm_"+parts[1]+"_"+parts[2]+"_"+parts[3]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdateHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: recordId, columnName: columnName, updateText: updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
            txt.style.display = 'none';
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
//lbl.style.minWidth='20px';
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
}
function updatedetailsdays(txtid,recordId,columname) {
//debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value.replace(/\s+$/, "");

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblday_"+parts[1]+"_"+parts[2]+"_"+parts[3]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdateHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: recordId, columnName: columnName, updateText: updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
            txt.style.display = 'none';
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
             initializetooltip();
// modify date milestone
try
{
var txtid="txtdate";
if(parts[1]!="0"&&parts[1]!="")
{
txtid+="_"+parts[1];
}
if(parts[2]!="0"&&parts[2]!="")
{
txtid+="_"+parts[2];
}
 txtid+="_7";
settimeline(txtid);
}
catch
{}
// end  modify date
//lbl.style.minWidth='20px';
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
}
function updatedetailstask(txtid,recordId,columname) {
//debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value;

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblm_"+parts[1]+"_"+parts[2]+"_"+parts[3]+"_"+parts[4]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdatetaskHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: recordId, columnName: columnName, updateText: updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
             txt.style.display='none';
            //lbl.textContent = updatedText;
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
        }
function updatedetailstaskdays(txtid,recordId,columname) {
//debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value;

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblday_"+parts[1]+"_"+parts[2]+"_"+parts[3]+"_"+parts[4]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdatetaskHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: recordId, columnName: columnName, updateText: updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
             txt.style.display='none';
            //lbl.textContent = updatedText;
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
// modify date task
try
{
var txtid="txtdate";
if(parts[1]!="0"&&parts[1]!="")
{
txtid+="_"+parts[1];
}
if(parts[2]!="0"&&parts[2]!="")
{
txtid+="_"+parts[2];
}
if(parts[3]!="0"&&parts[3]!="")
{
txtid+="_"+parts[3];
}
 txtid+="_7";
settimeline(txtid);
}
catch
{}
//end modify date

        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
        }
        function updatedetailssubtask(txtid,recordId,columname) {
debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value;

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblm_"+parts[1]+"_"+parts[2]+"_"+parts[3]+"_"+parts[4]+"_"+parts[5]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdatesubtaskHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id:recordId,columnName:columnName,updateText:updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
             txt.style.display='none';
            //lbl.textContent = updatedText;
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
//lbl.style.minWidth='20px';
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
}
function updatedetailssubtaskdays(txtid,recordId,columname) {
debugger
    var txt = document.getElementById(txtid);
    var updatedText = txt.value;

  var parts = txtid.split('_'); // ["txtm", recordId, columnName]
   var lbl=document.getElementById("lblday_"+parts[1]+"_"+parts[2]+"_"+parts[3]+"_"+parts[4]+"_"+parts[5]);
    var recordId = recordId;
    var columnName = columname;

    // Call server via AJAX (using fetch or XMLHttpRequest)
    fetch('UpdatesubtaskHandler.aspx', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id:recordId,columnName:columnName,updateText:updatedText })
    })
    .then(response => response.json())
    .then(data => {
//debugger
        if (data.success) {
//alert("Update successful");
             txt.style.display='none';
            //lbl.textContent = updatedText;
            if (updatedText != "") {
        if (updatedText.length > 18) {
            lbl.textContent = updatedText.substring(0, 18)+ "...";
        } else {
            lbl.textContent = updatedText;
        }
         lbl.setAttribute("data-bs-original-title", updatedText);
    }
             lbl.style.display='';
initializetooltip();
// modify date sub task
try
{
var txtid="txtdate";
if(parts[1]!="0"&&parts[1]!="")
{
txtid+="_"+parts[1];
}
if(parts[2]!="0"&&parts[2]!="")
{
txtid+="_"+parts[2];
}
if(parts[3]!="0"&&parts[3]!="")
{
txtid+="_"+parts[3];
}
if(parts[4]!="0"&&parts[4]!="")
{
txtid+="_"+parts[4];
}
 txtid+="_7";
settimeline(txtid);
}
catch{}
// end modify date
        } else {
            alert("Update failed: " + data.error);
        }
    });
    //.catch(err => console.error('Fetch error:', err));
   
}
function initializetooltip() {
    try {
        const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');

        tooltipElements.forEach(function (el) {

            // Avoid duplicate initialization
            if (!bootstrap.Tooltip.getInstance(el)) {
                new bootstrap.Tooltip(el, {
                    placement: 'top',
                    trigger: 'hover'
                });
            }

        });

    } catch (e) {
        console.error('Tooltip init error:', e);
    }
}
function initializedatepker() {
    try {
//alert('reset');
        $('.datepicker').each(function () {

            // Destroy if already initialized
            if ($(this).hasClass('hasDatepicker')) {
                $(this).datepicker('destroy');
            }

            // Reinitialize datepicker
            $(this).datepicker({
                dateFormat: "dd/mm/yy",
                duration: "fast",
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText) {
                    $(this).val(dateText);
                    $(this).trigger('blur');
                    $(this).datepicker("hide");
                    
                }
            });

        });

    } catch (e) {
        console.error('datepicker init error:', e);
    }
}
function addrowUpDown(straddon,currentindex,strpid,strmid,strorderno)
{
//alert("yes");
//debugger
$.ajax({
                    url: '/pms/makemilestone.aspx/addnewmilestoneblank',
                    data: "{ 'addon': '" + straddon + "','currentindex': '" + currentindex + "','pid': '" + strpid + "','mid': '" + strmid + "','orderno': '" + strorderno + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success:function (response) { 
//debugger                      
                       // alert(response.d);
                        if(response.d!="0"&&response.d!="")
                        {
                            var mid=response.d;
                         //toggleHiddenFieldValue(strpid,strmid,'') 
                         Bindtask_data();

                       // if(strmid!=""&&strmid!="0")
                       // {
//debugger


viewalldetails();
edittext('lblm_'+strpid+'_'+mid+'_1');
                         
                      //  }

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
function viewtask(strpid,strmid)
{
//debugger
document.getElementById('btn_'+strpid+'_'+strmid).click()
document.getElementById('btn_'+strpid+'_'+strmid).click()
document.getElementById('showtrM_'+strmid).click();
toggleTaskRows('taskshow_'+strmid);
    
}
function viewtask1(strpid,strmid)
{
//debugger

toggleTaskRows('taskshow_'+strmid);
    
}
function addrowUpDowntask(straddon,currentindex,strpid,strmid,strtaskid,strorderno)
{
//alert("yes");
if(strorderno=="0")
{
toggleHiddenFieldValue(strpid,strmid,'','add');
}
//debugger
$.ajax({
                    url: '/pms/makemilestone.aspx/addnewmilestoneblanktask',
                    data: "{ 'addon': '" + straddon + "','currentindex': '" + currentindex + "','pid': '" + strpid + "','mid': '" + strmid + "','taskid': '" + strtaskid + "','orderno': '" + strorderno + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success:function (response) { 
//debugger                      
                       // alert(response.d);
                        if(response.d!="0"&&response.d!="")
                        { 
var tid=response.d;      
              
                         Bindtask_data();


//debugger



viewalldetails();
edittexttask('lblm_'+strpid+'_'+strmid+'_'+tid+'_1');
debugger
initializedatepker();
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
function viewsubtask(strpid,strmid,strtaskid)
{
//debugger
    document.getElementById('btn_'+strpid+'_'+strmid).click()
    document.getElementById('showtrM_'+strmid).click();
    toggleTaskRows('taskshow_' + strmid);
    document.getElementById('btn_' + strpid + '_' + strmid+'_'+strtaskid).click()
    document.getElementById('showtasktr_'+strtaskid).click();
    toggleSubTaskRows('subtaskshow_' + strtaskid);
    
}
function viewsubtask1(strpid,strmid,strtaskid)
{
//debugger
    
    //toggleTaskRows('taskshow_' + strmid);
    toggleSubTaskRows('subtaskshow_' + strtaskid);
    
}
function addrowUpDownsubtask(straddon,currentindex,strpid,strmid,strtaskid,strsubtaskid,strorderno)
{
//alert("yes");
if(strorderno=="0")
{
toggleHiddenFieldValue(strpid,strmid,strtaskid,'add');
}
//debugger
$.ajax({
                    url: '/pms/makemilestone.aspx/addnewmilestoneblanksubtask',
                    data: "{ 'addon': '" + straddon + "','currentindex': '" + currentindex + "','pid': '" + strpid + "','mid': '" + strmid + "','taskid': '" + strtaskid + "','subtaskid': '" + strsubtaskid + "','orderno': '" + strorderno + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success:function (response) { 
//debugger                      
                       // alert(response.d);
                        if(response.d!="0"&&response.d!="")
                        { 
var subtid=response.d;                        
                         Bindtask_data();

//showtrnew(strmid);
                       // if(strmid!=""&&strmid!="0")
                       // {
//debugger

var newindx=0;
 var crindex=0;

viewalldetails();
edittextsubtask('lblm_'+strpid+'_'+strmid+'_'+strtaskid+'_'+subtid+'_1');


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


    </script>
 </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:HiddenField ID="hiddenDatePicker" runat="server" />
    <div id="loading" style="
    position: fixed;
    width: 100%;
    height: 100%;
    background: rgba(255,255,255,0.8);
    z-index: 9999;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
">
    <img src="/pms/image/loading8.gif" alt="Loading..." />
    
</div>
    <script>
$(document).ready(function() {
    $('#loading').fadeOut('slow');
});
$(window).on('load', function() {
    // Hide the loading overlay when page + all resources have loaded
    $('#loading').fadeOut('slow');
});
    </script>
    <div class="offcanvas-header border-bottom" style="padding:4px 24px;">
        
        <asp:HiddenField ID="hdnT" runat="server" />
        <asp:HiddenField ID="hdnST" runat="server" />
        <h5 class="offcanvas-title"><%=Projectname%></h5>

       <%-- <div class="offcanvas-header border-bottom" style="padding:22px 24px;">
        <h5 class="offcanvas-title"></h5>--%>
        <div class="btn-group mt-4 mt-sm-0" style="padding-right:70px;" role="group" aria-label="Basic example">
                                   
                                      
                                       <%-- <a href="javascript:void(0);" id="add1" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmile"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Milestone"><i class="ri-add-line"></i></span></a>--%>
                     <%if (ischeckViewM == 1)
                         {%>
            <a href="javascript:void(0);" id="add2"  onclick='addrowUpDown("2","0",<%=Request.QueryString["id"].ToString()%>,"0","0")' class="btn btn-outline-primary btn-icon" ><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Milestone"><i class="ri-add-line"></i></span></a>
            
            <%}%>                   
                                       
            
            <a href="javascript:void(0);" style="display:none" id="addview"  onclick='addviewreset()' class="btn btn-outline-primary btn-icon" ><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Reset"><i class="ri-add-line"></i></span></a>

                                           
                                       

                                        
                                           
                                        
                                        <button  type="button" id="btnsetting" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" data-bs-auto-close="outside"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Show/Hide Column" data-bs-original-title="Show/Hide Column"><i class="ti ti-settings"></i></span></button>
                                        <div class="dropdown-menu show_column" data-simplebar style="max-height:300px;">

                                          <a style="display:none;" class="dropdown-item toggle-vis" runat="server"  id="multi0" data-column="0"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("S.No", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi1" data-column="1"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Action", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi2" data-column="2"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Milestone", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi3" data-column="3"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi4" data-column="4"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Priority", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi5" data-column="5"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Completion Status", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi6" data-column="6"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Duration", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi7" data-column="7"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Start Date", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi8" data-column="8"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Task (Hour)", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi9" data-column="9"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Timeline", "2")%></a>
                                                 <a class="dropdown-item toggle-vis" runat="server"  id="multi10" data-column="10" style="display:none;"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Extended Timeline", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi11" data-column="11"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Owner", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi12" data-column="12"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Team", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi13" data-column="13"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Weightage", "2")%></a>
                                       
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi14" data-column="14"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Issues", "2")%></a>
                                           <a class="dropdown-item toggle-vis" runat="server"  id="multi15" data-column="15" style="display:none;"><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Stage", "2")%></a>
                                            <a class="dropdown-item toggle-vis" runat="server"  id="multi16" data-column="16" ><i class="ri-check-line align-middle fs-lg"></i><%=fns.getltext("Note", "2")%></a>
                                            

                                        </div>
                                      <div class="btn-group" role="group">
                                                <button id="btnGroupDrop1" type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-expanded="false"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="Change View"><i class="ri-grid-fill"></i></span></button>
                                                <ul class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                                    <li>
                                                       
                                                        <%--<a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/makemilestone.aspx<%=strquery%>')">List1</a>--%>
                                                        <a class="dropdown-item" href="javascript:void(0);" type="button" onclick="hidepopup('1');">List</a>
                                                    </li>
                                                    <li>
                                                        
                                                        <%--<a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/gantt.aspx<%=strquery%>')">Gantt1</a>--%>
                                                        <a class="dropdown-item" href="javascript:void(0);" type="button" onclick="hidepopup('2');">Gantt</a>
                                                    </li>
                                                </ul>
                                            </div>
                                     
                                    </div>
        <div class="hstack gap-2 cls_btn" style="top:11px;">
            <button type="button" id="zoominidmulti" onclick="zoominmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutidmulti" onclick="zoomoutmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmainmulti()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
    <div class="offcanvas-body p-0 overflow-hidden">
       <%-- <div data-simplebar style="height: calc(100vh - 57px);">   --%>
        <div style="height:100%;">   
            <div class="acitivity-timeline p-0">
                <div class="card border-0 mb-0">
                    

<script>

</script>
<style>
/*td.details-control {
    background: url('/pms/assets/images/open.png') no-repeat center right;
    cursor: pointer;
}
td.shown {
    background: url('/pms/assets/images/close.png') no-repeat center right;

}*/
.details-control {
    background: url('/pms/assets/images/open.png') no-repeat center right;
    cursor: pointer;
}
.shown {
    background: url('/pms/assets/images/close.png') no-repeat center right;

}
.datatable tr th:first-child, .datatable tr td:first-child {display:none;}
    </style>



                   <%--     <div class="card-header" style="display:none;">
                            <div class="row">
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addmile"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Milestone"><i class="ri-add-line"></i></span></a>
                                       
                                    </div>
                                    </div>
                                </div>                                
                            </div> 
                        </div>--%>
                    <div id="dvlist">
                      <%--<div class="card-header">
                                      <div class="row">
                                     <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    
                                    </div>
                                </div>
                                 </div> 
                                 </div>--%>
                        <div class="card-body pb-0">
                            <div class="table-responsive">
                                 <asp:UpdatePanel ID="UpdatePanel42" runat="server">                                          
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdnM" runat="server" />
                                  <div id="divgridview" style="width:100%;">
                             <asp:Label ID="lblprojectdata" runat="server"></asp:Label>
                                      </div>
                                               

                                             </ContentTemplate>
                                                </asp:UpdatePanel>
                             <div style="display:none;">
                              <asp:Button ID="btndelete" runat="server"    Text="delete"  />
                             </div>
                            </div>

                        </div>

                    </div>






<%--Task add--%>
<div class="offcanvas offcanvas-end" id="addtask" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
             <label id="lbltitle" runat="server"><%=fns.getltext("Add Task", "2")%></label>   

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="showdivimg()" aria-label="Close"><i class="ph-x-bold"></i></button>
             <div style="display:none"><button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
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
                                     <asp:Button ID="btnload" runat="server" Text="" OnClick="btnload_Click" />
                                 </div>
                                    <asp:HiddenField ID="hdnid" runat="server" Value="0" />
                                   <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnmilstoneid" runat="server" Value="0" />

                                   <div class="col-xxl-12 col-lg-12" id="divproject" >
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="ProjectId" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                      <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectId" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" AutoPostBack="true" OnSelectedIndexChanged="ProjectId_SelectedIndexChanged">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectId" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divmilestone">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%>&nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Milestoneid" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true"  OnSelectedIndexChanged="Milestoneid_SelectedIndexChanged" onchange="GetWeitagebindDtls()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnMilestoneid" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnweightage" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divptasklist" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="tasklist" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdntasklist" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>



                                    <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                       <label class="form-label flex-grow-1"><%=fns.getltext("Task Name", "2")%></label>
                                    </div>
                                    <input type="text" id="taskname" runat="server" placeholder="Task Name" class="form-control">
                                </div>



                               <div class="col-xxl-12 col-lg-12" id="divmiletimeline" style="display:none;">
                              <asp:Label ID="lblmiletimeline" runat="server"></asp:Label>
                              </div>

                             <div class="col-xxl-12 col-lg-12" id="divtasktimeline" style="display:none;">
                           <asp:Label ID="lbltasktimeline" runat="server"></asp:Label>
                            </div>



                                 <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="issubtask" runat="server" />  Has Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                    <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkTaskWeightage" runat="server"  />  Do You Want Save Weightage on Sub Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                               



                                
                             
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Description", "2")%></label>
                                    </div>
                                    <textarea id="taskdescripton" runat="server" class="form-control" placeholder="Task Description"></textarea>
                                </div>


                                  <div class="col-xxl-12 col-lg-12" id="divweitage">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightage" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchk(this);" class="form-control">
                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      


                                </div>

                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>&nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Startdate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>

                                     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Startdate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">

                                               <asp:HiddenField ID="hdnstartdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>

                              <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="StartTime" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnstarttime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>




                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Target Date", "2")%>&nbsp;&nbsp;

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator3" 
                                                ControlToValidate="Targetdate" ValidationGroup="btnsave"
                                                                runat="server"  ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Targetdate" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate(this.id)">
                                               <asp:HiddenField ID="hdntargetdate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>


                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%>&nbsp;&nbsp;</label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Timeline" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTimeline" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration (Hour)", "2")%></label>
                                    </div>
                                    <input type="text" id="taskDuration" runat="server" onkeypress="return isnumeric(event)" placeholder="Hour" onkeyup="checkdurationcalculate(this);" class="form-control">
                                </div>

                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="P_Status" InitialValue="0" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Status" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatus()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewprojectstatus" style="display:none">
                                          <input type="text" id="txtnewprojectstatus" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Status" runat="server" Value="0" />
                                         



                                </div>



                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                              <asp:AsyncPostBackTrigger ControlID="btnload" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                              </ContentTemplate>
                                        </asp:UpdatePanel>
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                          

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budget" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actual" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%> &nbsp;&nbsp;</label>
                                    </div>
                                    
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Reminder" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetReminderDtls()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnreminder" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divremindertype">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%></label>
                                    </div>
                                
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ReminderType" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="GetRemindertypeDtls()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                         </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnReminderType" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="col-xxl-12 col-lg-12"  id="divDays">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Enter Day(s)", "2")%></label>
                                    </div>
                                    <input type="text" id="Days" runat="server" placeholder="Days" class="form-control">
                                     </div>


                                <div class="col-xxl-12 col-lg-12" id="divChooseDate">
                                 <div class="d-flex align-items-center">
                                 <label class="form-label flex-grow-1"><%=fns.getltext("Choose Date", "2")%></label>
                                 </div>
                                
                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="ChooseDate" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnChooseDate" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>


                                </div>

                                    <div class="col-xxl-12 col-lg-12" id="Divtime">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Time", "2")%></label>
                                    </div>
                                   
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Time" runat="server" class="form-control">
                                         <asp:HiddenField ID="hdnTime" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                   <div class="col-xxl-12 col-lg-12"  id="divNotifyusers">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Notify users", "2")%></label>
                                    </div>
                                
                                      <asp:DropDownList ID="Notifyusers" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Notify User ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnNotifyusers" runat="server" Value="0" />
                                     
                                    </div>
                                

                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Occurrence", "2")%></label>
                                    </div>
                                    <input type="text" id="Occurrence" runat="server" placeholder="Occurrence"  class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Tag To", "2")%></label>
                                    </div>
                                    <input type="text" id="tagto" runat="server" placeholder="Tag To" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                   <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>--%>
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                       <%--    </ContentTemplate>
                                        </asp:UpdatePanel>--%>


                                </div> 
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Stage", "2")%></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Stage" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Stage ..." AllowSingleDeselect="true" onchange="AddNewStage()">
                                                </cc1:DropDownListChosen>
                                               </ContentTemplate>
                                        </asp:UpdatePanel>
                                              <div id="dvnewStage" style="display:none">
                                          <input type="text" id="txtnewstage" class="form-control" placeholder="Stage"  runat="server" />
                                                  <input  data-jscolor="{}" id="txtstageprintcolor" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnstage" runat="server" Value="0" />
                                        

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

   <%-- <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Save</a>
        <a href="javascript:void(0);" class="btn btn-secondary">Save & Add More</a>
    </div>--%>

    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img2" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>


</div>

<%--subtask add--%>
                    


<%--milestone add--%>
<div class="offcanvas offcanvas-end" id="addmile" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="lbltitlemile" runat="server"><%=fns.getltext("Add Milestone", "2")%></label>  

        </h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  onclick="showdivimgmile()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclsmile" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
    <%--<div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">--%>
                             <iframe id='editdetails' src="" width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>
 		                   
                            <div class="row" style="display: none">
                                 <div style="display: none">
                                     <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                          <ContentTemplate>
                                     <asp:Button ID="Button1mile" runat="server" Text="" OnClick="Button1mile_Click" />
                                             <asp:Button ID="btnloadmile" runat="server" Text="" OnClick="btnloadmile_Click" />
                                              </ContentTemplate>
                                         </asp:UpdatePanel>
                                     
                                 </div>

                                    <asp:HiddenField ID="hdnidmile" runat="server" Value="0" />
                                <asp:HiddenField ID="hdiprojidmile" runat="server" Value="0" />
                                <div class="col-xxl-12 col-lg-12" id="divprojectmile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="ProjectIdmile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>


                                        </label>
                                    </div>
                                 
                                    <%--onchange="GetWeitagebindDtls()"--%>
                                      <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                              
                                          </Triggers>
                                         <ContentTemplate>
                                       <cc1:DropDownListChosen ID="ProjectIdmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project ..." AllowSingleDeselect="true" onchange="GetWeitagebindDtlsmile()">
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdnProjectIdmile" runat="server" Value="0" />
                                              <asp:HiddenField ID="hdnweightagemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Milestone" ValidationGroup="btnsavemile"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Milestone" placeholder="Milestone" runat="server"  class="form-control">
                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divprojecttimeline" style="display:none;">
                                 <asp:Label ID="projecttimeline" runat="server"></asp:Label>
                                 </div>



                                 <div class="col-xxl-12 col-lg-12" id="divmiletimeline_mile" style="display:none;">
                                  <asp:Label ID="lblmiletimeline_mile" runat="server"></asp:Label>
                                  </div>



                                  <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       
                                        <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                              <input class="form-check-input" type="checkbox" id="istask" runat="server" />Has Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-xxl-10 col-lg-12">
                                    <div class="d-block align-items-center">
                                       <div class="form-check" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover">
                                            <label class="form-label">
                                             <input class="form-check-input" type="checkbox" id="checkmileWeightage" runat="server"  />  Do You Want Same Weightage on Task 
                                        </label>
                                        </div>
                                    </div>
                                </div>


                              


                               
                                <div class="col-xxl-12 col-lg-12" id="divweitagemile">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Weightage %", "2")%></label>
                                    </div>
                                    <input type="text" id="Weightagemile" placeholder="Weightage" onkeypress="return isnumeric(event)" runat="server" onkeyup="waitagepercentchkmile(this);" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="StartDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                       <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <%--<input type="text" id="StartDate" runat="server" class="form-control">checkcomparisionmilestonedate()--%>
                                      <input type="text" id="StartDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <asp:HiddenField ID="hdnStartDatemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Date", "2")%>

                                            <%--<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="EndDate" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>

                                      <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="EndDatemile" runat="server" class="form-control" onchange="checkdatecomparisiontaskdate_mile(this.id)">
                                         <%--<input type="text" id="EndDate" runat="server" class="form-control">--%>
                                             <asp:HiddenField ID="hdnEndDatemile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>
                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>

                                      <asp:DropDownList ID="Ownermile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnownermile" runat="server" Value="0" /> 
  

                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="teammile" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hdnteammile" runat="server" Value="0" />
                                      


                                </div>


                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Status", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator10" 
                                                ControlToValidate="P_Statusmile" InitialValue="0" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                   
                                     <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="P_Statusmile" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Project Status ..." AllowSingleDeselect="true" onchange="AddNewProjectstatusmile()">
                                                </cc1:DropDownListChosen>
                                              <div id="dvnewprojectstatusmile" style="display:none">
                                          <input type="text" id="txtnewprojectstatusmile" class="form-control" placeholder="Status"  runat="server" />
                                                  <input  data-jscolor="{}" id="stustprintColormile" class="jscolor form-control" runat="server">
                                                  </div>
                                                <asp:HiddenField ID="hdnP_Statusmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>



                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator11" 
                                                ControlToValidate="Prioritymile" InitialValue="0" Display="Dynamic" ValidationGroup="btnsavemile" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                       <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                               <asp:AsyncPostBackTrigger ControlID="btnloadmile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Prioritymile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Status ..." AllowSingleDeselect="true" onchange="AddNewprioritymile()">
                                         </cc1:DropDownListChosen>
                                                 <div id="dvnewprioritymile" style="display:none">
                                          <input type="text" id="txtnewprioritymile" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolormile" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPrioritymile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Budget", "2")%></label>
                                    </div>
                                    <input type="text" id="Budgetmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Budget" class="form-control">
                                </div>

                                  <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Actual", "2")%></label>
                                    </div>
                                    <input type="text" id="Actualmile" runat="server" onkeypress="return isnumeric(event)" placeholder="Actual" class="form-control">
                                </div>



                                <div class="col-xxl-12 col-lg-12" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Access", "2")%>  &nbsp;&nbsp;
                                            
                                           <%-- <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator2" 
                                                ControlToValidate="Access" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="Accessmile" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Staff</asp:ListItem>
                                          <asp:ListItem  value="2">Client</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                               <asp:HiddenField ID="hdnAccessmile" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>    
                                


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Duration Type", "2")%>  &nbsp;&nbsp;
                                            
                                        </label>
                                    </div>
                                  
                                    <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button1mile" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>

                                     <cc1:DropDownListChosen ID="ddldurationtype" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task Duration ..." AllowSingleDeselect="true" onchange="GetDurationchange()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                         <asp:ListItem  value="1">Hours</asp:ListItem>
                                          <asp:ListItem  value="2">Days</asp:ListItem>
                                          <asp:ListItem  value="3">Month</asp:ListItem>
                                                </cc1:DropDownListChosen>
                                             <asp:HiddenField ID="hdndurationtype" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>

                                </div>


                                 <div class="col-xxl-12 col-lg-12" id="divchngtype" style="display:none;">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task Duration", "2")%> <span id="chngtype"></span></label>
                                    </div>
                                    <input type="text" id="taskDurationmile" runat="server" onkeypress="return isnumeric(event)"  class="form-control">
                                </div>


                            </div>
                        <%--</div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    
      <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right" style="display:none;"> 
         <div id="pnlimgmile" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img3" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsubmile" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsavemile" onclick="SvaeDatamile('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmoremile" onclick="SvaeDatamile('savemore');">Save & Add More</a>
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimgmile()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclosemile" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>
    <script type="text/javascript">

            ////debugger
            var count = 1;
            function loadsrcedit(valframe ,valsrc,stredit) {
                //debugger
                
                // alert(lblname);
                count++;
                
                try{
                document.getElementById(valframe).src = "";
                  
                }
                catch
                {}
                if (valsrc.includes('?')) {
                    document.getElementById(valframe).src = valsrc +"&count=" + count;
                }
                else {
                    document.getElementById(valframe).src = valsrc + "?count=" + count;
                }

                try {

                    document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML = stredit;
                }
                catch {
                }
            }

      function loadsrcfiles(valframe ,valsrc,strtype) {
                debugger
                
                // alert(lblname);
                count++;
                var callfrom="&callfrom=makemilestone";
                try{
                document.getElementById(valframe).src = "";
                  
                }
                catch
                {}
                if (valsrc.includes('?')) {
                    document.getElementById(valframe).src = valsrc +"&count=" + count;
                }
                else {
                    document.getElementById(valframe).src = valsrc + "?count=" + count;
                }
          document.getElementById(valframe).src=document.getElementById(valframe).src+callfrom;
                try {
                    if(strtype=="milestone")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload milestone documents";
if(strtype=="task")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload task documents";
if(strtype=="subtask")
                    document.getElementById('<%=lblfiles.ClientID%>').innerHTML = "Upload sub task documents";
                }
                catch {
                }
            }
        </script>

</div>





<div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>

<div id="changeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="btnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="hdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          
                               <asp:DropDownList ID="ddlchangeowner" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdncahngeowner" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveowner" class="btn btn-primary" onclick="SvaeDataowner('saveowner');">Save</button>
            </div>
        </div>
    </div>
</div>


<div id="milechangeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="milebtnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="milehdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          
                               <asp:DropDownList ID="mileddlchangeowner" CssClass="form-control" runat="server" 
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="milehdncahngeowner" runat="server" Value="0" />
                                             <asp:HiddenField ID="milehdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveowner" class="btn btn-primary" onclick="mileSvaeDataowner('milesaveowner');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="sbtskchangeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" id="sbtskbtnclsowner" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                      <asp:HiddenField ID="sbtskhdnchangeid" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                           

                               <asp:DropDownList ID="sbtskddlchangeowner" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="sbtskhdncahngeowner" runat="server" Value="0" />
                                             <asp:HiddenField ID="sbtskhdnchnownrghtml" runat="server" Value="0" />




                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="sbtskbtnsaveowner" class="btn btn-primary" onclick="sbtskSvaeDataowner('sbtsksaveowner');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="addteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="btnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="hdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                         <%--   <select id="select_emp1" class="form-control" multiple="multiple">
                                <option value="1" selected="selected">Christi</option>
                                <option value="2" selected="selected">Frank Hook</option>
                                <option value="3" selected="selected">Ruby</option>
                                <option value="4" selected="selected">Vivek Sisodia</option>
                                <option value="5" selected="selected">Avneesh</option>
                                <option value="6">Mohit Sharma</option>
                                <option value="7">Amit Kumar</option>
                                <option value="8">Jone Doe</option>
                                <option value="9">Avdhesh Singh</option>
                                <option value="10">Ranjay Varun</option>
                                <option value="11">Sachin Gupta</option>
                                <option value="12">Sanjeev</option>
                            </select>--%>

                             <asp:DropDownList ID="ddladdteam" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnaddteam" runat="server" Value="0" />
                                             <asp:HiddenField ID="hdnaddteamhtml" runat="server" Value="0" />
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveteam" class="btn btn-primary" onclick="SvaeDatateam('saveteam');">Save</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

<div id="mileaddteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="milebtnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="milehdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                        
                             <asp:DropDownList ID="mileddladdteam" CssClass="form-control" runat="server" 
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="milehdnaddteam" runat="server" Value="0" />
                                            <asp:HiddenField ID="milehdnaddteamhtml" runat="server" Value="0" />

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveteam" class="btn btn-primary" onclick="mileSvaeDatateam('milesaveteam');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="sbtskaddteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" id="sbtskbtnclsTeam" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                         <asp:HiddenField ID="sbtskhdnteameid" runat="server" Value="0" />
                        <div class="col-sm-12 mb-1">
                         

                             <asp:DropDownList ID="sbtskddladdteam" CssClass="form-control" runat="server" width="650px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Change Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="sbtskhdnaddteam" runat="server" Value="0" />
                                             <asp:HiddenField ID="sbtskhdnaddteamhtml" runat="server" Value="0" />
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="sbtskbtnsaveteam" class="btn btn-primary" onclick="sbtskSvaeDatateam('sbtsksaveteam');">Save</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="addfiles" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content" style="height:500px;">
           <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">
                    <asp:Label ID="lblfiles" runat="server" Text="Upload Files"></asp:Label></h5>
                <button type="button" class="btn-close" id="closfiles" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                          <iframe id='inner_popupfiles' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
            
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
                    <div id="setdurationtime" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content" style="height:100px;">
           <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">
                    <asp:Label ID="Label1" runat="server" Text="Time Duration"></asp:Label></h5>
                <button type="button" class="btn-close" id="closduration" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <uc1:setdurationtime runat="server" ID="setdurationtime1" />         
            
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

                         <asp:HiddenField ID="hdnprojec_tid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnmilestone_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasklist_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntask_id" runat="server" Value="0" />
                         <asp:HiddenField ID="hdntransaction_id" runat="server" Value="" />
                          <asp:HiddenField ID="hdnstoptimer_id" runat="server" Value="" />

                          <asp:HiddenField ID="hdnprojecnots_tid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnmilestonenotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasklistnotes_id" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntasknotes_id" runat="server" Value="0" />
                         <asp:HiddenField ID="hdnnotestransaction_id" runat="server" Value="" />
                          <asp:HiddenField ID="hdnpause_id" runat="server" Value="" />




<div id="ExtendedTimline_task" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended_task" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button4" runat="server" Text="" OnClick="Button4_Click" />
   </div>
                      <asp:HiddenField ID="hdnextendid_task" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml_task" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendatestrt_task" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt_task" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button4" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendateend_task" runat="server" class="form-control">
                                              <asp:HiddenField ID="hdnextndend_task" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveextended_task" class="btn btn-primary" onclick="SvaeExtended_task('saveExtended_task');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="MileExtendedTimline" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="milebtnclsextended" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />
   </div>
                      <asp:HiddenField ID="milehdnextendid" runat="server" Value="0" />
                          <asp:HiddenField ID="milehdnextnhtml" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="mileExtendatestrt" runat="server" class="form-control">
                                               <asp:HiddenField ID="milehdnextndstrt" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="mileExtendateend" runat="server" class="form-control">
                                              <asp:HiddenField ID="milehdnextndend" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="milebtnsaveextended" class="btn btn-primary" onclick="mileSvaeExtended('milesaveExtended');">Save</button>
            </div>
        </div>
    </div>
</div>



<div id="ExtendedTimline_subtask" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended_subtask" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <asp:Button ID="Button5" runat="server" Text="" OnClick="Button5_Click" />
   </div>
                      <asp:HiddenField ID="hdnextendid_subtask" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml_subtask" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendatestrt_subtask" runat="server" class="form-control">
                                               <asp:HiddenField ID="hdnextndstrt_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="Button5" EventName="Click" />
                                          </Triggers>
                                         <ContentTemplate>
                                    <input type="text" id="Extendateend_subtask" runat="server" class="form-control">
                                              <asp:HiddenField ID="hdnextndend_subtask" runat="server" Value="0" />
                                           </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" id="btnsaveextended_subtask" class="btn btn-primary" onclick="SvaeExtended_subtask('saveExtended_subtask');">Save</button>
            </div>
        </div>
    </div>
</div>

<div id="actualbudget" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Project Cost Sheet Actual Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                                              
                    </div>
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<script>
    function fsFunction(){
        var element = document.getElementById("addtask");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunction(){
        var element = document.getElementById("addtask");
        element.classList.remove("canvas-fullscreen");
    }
</script>

<script>
    function fsFunctionsubtask(){
        var element = document.getElementById("addsubtask");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunctionsubtask(){
        var element = document.getElementById("addsubtask");
        element.classList.remove("canvas-fullscreen");
    }
</script>


<script>
    function fsFunctionmile(){
        var element = document.getElementById("addmile");
        element.classList.add("canvas-fullscreen");  
    }
    function fseFunctionmile(){
        var element = document.getElementById("addmile");
        element.classList.remove("canvas-fullscreen");
    }
</script>

    
<%--<script>
    $('#select_emp1').select2({
        dropdownParent: $("#addteam"),
        placeholder: 'Select Employee',
	    width: '100%',
        closeOnSelect: false,
        allowClear: true,
    });   
</script>--%>
<%--<script>
    $('#select_emp2').select2({
        dropdownParent: $("#addtask"),
        placeholder: 'Select Employee',
	    width: '100%',
        closeOnSelect: false,
        allowClear: true,
    });   
</script>--%>




                           

<asp:HiddenField ID="hdntype" runat="server" />

<script>
   

    if (document.getElementById('<%=hdntype.ClientID%>').value == "1")
    {
        document.getElementById("divproject").style.display = "none";
        document.getElementById("divprojectmile").style.display = "none";
        document.getElementById("divproject").style.display = "none";

          var RecordId=document.getElementById("<%=ProjectIdmile.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_mile',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise_mil,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise_mil(response) {
            
            //debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "block";

                     }
                    
                }

            }
            catch

            {

            }


         //Owner Bind Project Wise;
         //debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwnermile = $(document.getElementById("<%=Ownermile.ClientID%>"));
                                                         $(ddLOwnermile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=Ownermile.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=Ownermile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownermile.ClientID %>").value = ids;
                                                        document.getElementById("<%=Ownermile.ClientID%>").click();

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





         //Team Bind Project Wise;
         //debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteammile = $(document.getElementById("<%=teammile.ClientID%>"));
                                                         $(ddLteammile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teammile.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=teammile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteammile.ClientID %>").value = ids;
                                                        document.getElementById("<%=teammile.ClientID%>").click();

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



        // project tomeline //
           //debugger
            document.getElementById("divprojecttimeline").style.display = "block";
            var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindProjecttimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessproject_timeline_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        
        function OnSuccessproject_timeline_mile(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=projecttimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }





       //  Milestone Timeline //
              //debugger
              document.getElementById("divmiletimeline_mile").style.display = "block";
              var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
              //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata_mile',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccessgetdetails_miletimelinedata_mile(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }
        
      }
    

</script>





<script>


     function GetOwnerempdetails() {
        //debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

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





     function GetTeamempdetails() {
        //debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=team.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=team.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=team.ClientID%>").click();

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




   
    function GetWeitagebindDtls() {

                  var RecordId=document.getElementById("<%=Milestoneid.ClientID%>").value;

                  $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschk(response) {
            
            //debugger
            try {
                var waitagechkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls.find("id").text() != "")
                {
                    //alert(waitagechkdtls.find("checkprojectWeightage").text());
                      if (waitagechkdtls.find("checkmileWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls.find("checkmileWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }

             GetOwnerempdetails();
            GetTeamempdetails();

             document.getElementById("divmiletimeline").style.display = "block";
            Bindmilestonetimeline_data();
             document.getElementById("divtasktimeline").style.display = "block";
            Bindtasktimeline_data();


         }


       
    }
    

</script>

<script>
   
    function waitagepercentchk(input)
    {
        
        if (input.value > 100)
        {
            
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>


<script>
   
    function checkdurationcalculate(input)
    {

        var Milestoneid = document.getElementById("<%=Milestoneid.ClientID%>").value;

       

                  $.ajax({
                    url: 'MultiProjectTask.aspx/BindcheckTaskdurationdetails',
                    data: "{ 'Milestoneid': '" + Milestoneid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessChktaskdurationlschk,
                    error: function (response) {
                     alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessChktaskdurationlschk(response) {
            
            //debugger
           // alert(response.d);
            try {
                var Taskduraionchkdtls = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Taskduraionchkdtls.find("Durationhourcal").text() != "")
                {

                    var Task_Duration = Taskduraionchkdtls.find("TaskDuration").text();
                    var Duration_hourcal = Taskduraionchkdtls.find("Durationhourcal").text();

                  
                    //alert(Task_Duration);
                    //alert(Duration_hourcal);
                    //alert(input.value);

                    if (Duration_hourcal > Task_Duration)
                    {
                        //alert('yes');
                        if (Task_Duration != 0)
                        {
                            if (parseInt(input.value) > Task_Duration) {

                                alert('Milestone hour can not greater task hour');
                                input.value = 0;

                            }

                            var chktsk = parseInt(Task_Duration) + parseInt(input.value);
                            //alert(chktsk);
                            if (chktsk > Duration_hourcal) {

                                alert('Milestone hour can not greater task hour');
                                input.value = 0;

                            }
                        }
                    }

                    if (Duration_hourcal < parseInt(input.value))
                    {
                         alert('Milestone hour can not greater task hour');
                         input.value = 0;

                    }

                    if (Duration_hourcal == Task_Duration )
                    {
                        if (parseInt(input.value) > 0)
                        {
                            
                            alert('Milestone hour can not greater task hour');
                            input.value = 0;
                        }
                    }

                       
                  
                    
                    
                }
                   
            }
            catch

            {

            }

           

         }





      

  }
    

</script>



<script>
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }

</script>

<script>
  function GetReminderDtls()
  {
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }


    if (document.getElementById('<%=Reminder.ClientID%>').value == "1")
    {
            document.getElementById("divremindertype").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divChooseDate").style.display = "none";

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "2")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            document.getElementById("divDays").style.display = "none";

      }


  } 

</script>

<script>
  function GetRemindertypeDtls()
  {
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "2")
    {
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
    if (document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divDays").style.display = "none";
            document.getElementById("divChooseDate").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

      }

    if (document.getElementById('<%=Reminder.ClientID%>').value == "1" && document.getElementById('<%=ReminderType.ClientID%>').value == "3")
    {
            document.getElementById("divChooseDate").style.display = "none";
            document.getElementById("divDays").style.display = "block";
            document.getElementById("Divtime").style.display = "block";
            document.getElementById("divNotifyusers").style.display = "block";
            

    }
  } 

</script>

<script>
    function checkdate(enterdate)
    {
        if (document.getElementById("<%=Startdate.ClientID%>").value != "" && document.getElementById("<%=Targetdate.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=Startdate.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=Targetdate.ClientID%>").value);
                              var todt = new Date(today2);

                               var t1 = new Date();
                               t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                               var t2 = new Date();
            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
            if (t1 > t2) {
                 alert("Enter Valid Date");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }
            else {
return true;
            }
        }
        else
        {
            return true;
        }
    }
    function checkdatecomparisiontaskdate(enterdate) {
        //debugger
        if (enterdate != "") {

            if (checkdate(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

               var milesto_nid = document.getElementById("<%=Milestoneid.ClientID%>").value;



                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckstrtdatemiledetails',
                    data: "{ 'milesto_nid': '" + milesto_nid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        //debugger
                         var taskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (taskstrtdatechkdtls.find("id").text() != "")
                        {
                            const today = new Date(taskstrtdatechkdtls.find("StartDate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(taskstrtdatechkdtls.find("EndDate").text());
                            var todt = new Date(today2);

                            var t1 = new Date();
                            t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                            var t2 = new Date();
                            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
                            if (fcompdate >= t1 && fcompdate <= t2)
                            {
                                return true
                            }
                            else
                            {
                                alert("Enter Valid Date");
                                document.getElementById(enterdate).value = "";
                                document.getElementById(enterdate).focus();
                                return false;
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });


            }

           
        }
    }

    
    

</script>



<asp:HiddenField ID="hdnsavetype" runat="server" />
     <%--save data start --%> 

    <script type="text/javascript">
                function showdivimg() {
                    if (Page_ClientValidate("btnclose")) {
                        if (confirm('Do you want Leave this page ?')) {   

                            Cleardata();

                            document.getElementById("btnclose").click();
                                
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
        }



           function AddNewProjectstatus()
        {
                //debugger
                document.getElementById('<%=hdnP_Status.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatus").style.display = "none";
               if (document.getElementById('<%=P_Status.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatus").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatus.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColor.ClientID%>').value = "";
               }
        }


        function AddNewpriority()
        {
                //debugger
                document.getElementById('<%=hdnPriority.ClientID%>').value = "0";
                 document.getElementById("dvnewpriority").style.display = "none";
               if (document.getElementById('<%=Priority.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "1";
                    document.getElementById("dvnewpriority").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewpriority.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').value = "";
               }
        }


         function AddNewStage()
        {
                //debugger
                document.getElementById('<%=hdnstage.ClientID%>').value = "0";
                 document.getElementById("dvnewStage").style.display = "none";
               if (document.getElementById('<%=Stage.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnstage.ClientID%>').value = "1";
                    document.getElementById("dvnewStage").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewstage.ClientID%>').value = "";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').value = "";
               }
        }

      


          function SvaeData(val1A) {
                    //debugger
              if (Page_ClientValidate("btnsave"))
              {
                         document.getElementById('<%=hdnsavetype.ClientID%>').value = val1A;  
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimg").style.display = "block";
                            document.getElementById("divsub").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }


                       //status save
                        
                        var statusid = "0";
                        if (document.getElementById('<%=hdnP_Status.ClientID%>').value != "" && document.getElementById('<%=hdnP_Status.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatus.ClientID%>').value != "" && document.getElementById('<%=stustprintColor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/statussave",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatus.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        statusid = response.d;
                                    

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            statusid = document.getElementById('<%=P_Status.ClientID%>').value;
                        }

                        // Priority type save

                         var Prioritytypeid = "0";
                        if (document.getElementById('<%=hdnPriority.ClientID%>').value != "" && document.getElementById('<%=hdnPriority.ClientID%>').value != "0" && document.getElementById('<%=txtnewpriority.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddPrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewpriority.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        Prioritytypeid = response.d;
                                    

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            Prioritytypeid = document.getElementById('<%=Priority.ClientID%>').value;
                        }



                      // Stage save

                         var stageid = "0";
                        if (document.getElementById('<%=hdnstage.ClientID%>').value != "" && document.getElementById('<%=hdnstage.ClientID%>').value != "0" && document.getElementById('<%=txtnewstage.ClientID%>').value != "" && document.getElementById('<%=txtstageprintcolor.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddStage",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewstage.ClientID%>').value) + ',Stagecolor: ' + JSON.stringify(document.getElementById('<%=txtstageprintcolor.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        stageid = response.d;
                                    

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            stageid = document.getElementById('<%=Stage.ClientID%>').value;
                        }



                        // save all details


                        
                        var taskdetails = {};
                        taskdetails.id = document.getElementById('<%=hdnid.ClientID%>').value; 
                        taskdetails.ProjectId = document.getElementById('<%=ProjectId.ClientID%>').value; 
                        taskdetails.Milestoneid = document.getElementById('<%=Milestoneid.ClientID%>').value; 
                        taskdetails.tasklist = document.getElementById('<%=tasklist.ClientID%>').value; 
                        taskdetails.taskname = document.getElementById('<%=taskname.ClientID%>').value; 
                        taskdetails.taskdescripton = document.getElementById('<%=taskdescripton.ClientID%>').value; 
                        taskdetails.owner = document.getElementById('<%=hdnowner.ClientID%>').value; 
                        taskdetails.Startdate = document.getElementById('<%=Startdate.ClientID%>').value; 
                        taskdetails.Targetdate = document.getElementById('<%=Targetdate.ClientID%>').value;
                        taskdetails.StartTime = document.getElementById('<%=StartTime.ClientID%>').value; 
                        taskdetails.Timeline = document.getElementById('<%=Timeline.ClientID%>').value; 
                        taskdetails.TaskDuration  = document.getElementById('<%=taskDuration.ClientID%>').value; 
                        <%--taskdetails.P_Status = document.getElementById('<%=P_Status.ClientID%>').value; 
                        taskdetails.Priority = document.getElementById('<%=Priority.ClientID%>').value; --%>
                        taskdetails.P_Status = statusid;
                        taskdetails.Priority = Prioritytypeid;
                        taskdetails.Budget = document.getElementById('<%=Budget.ClientID%>').value;
                        taskdetails.Actual = document.getElementById('<%=Actual.ClientID%>').value;
                        taskdetails.Reminder = document.getElementById('<%=Reminder.ClientID%>').value; 
                        taskdetails.ReminderType = document.getElementById('<%=ReminderType.ClientID%>').value; 
                        taskdetails.Days = document.getElementById('<%=Days.ClientID%>').value; 
                        taskdetails.Time = document.getElementById('<%=Time.ClientID%>').value; 
                        taskdetails.Notifyusers = document.getElementById('<%=hdnNotifyusers.ClientID%>').value; 
                        taskdetails.ChooseDate = document.getElementById('<%=ChooseDate.ClientID%>').value; 
                        
                        taskdetails.Occurrence = document.getElementById('<%=Occurrence.ClientID%>').value; 
                        taskdetails.tagto = document.getElementById('<%=tagto.ClientID%>').value; 
                        taskdetails.team = document.getElementById('<%=hdnteam.ClientID%>').value;
                        taskdetails.Stage = stageid;
                       taskdetails.Weightage = document.getElementById('<%=Weightage.ClientID%>').value;
                   
                    if (document.getElementById('<%=issubtask.ClientID%>').checked == true)
                    {

                        taskdetails.issubtask = 1

                    }

                    if (document.getElementById('<%=issubtask.ClientID%>').checked == false)
                    {

                        taskdetails.issubtask = 0

                   }


                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == true)
                       {

                        taskdetails.checkTaskWeightage = 1

                      }

                      if (document.getElementById('<%=checkTaskWeightage.ClientID%>').checked == false)
                      {

                        taskdetails.checkTaskWeightage = 0

                      }

                  
                        $.ajax({
                            type: "POST",
                            url: 'makemilestone.aspx/SaveTask',
                            data: '{taskdetails: ' + JSON.stringify(taskdetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccess,
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                        return false;
                        //});


                    };
                }
                function OnSuccess(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                     document.getElementById('<%=btnload.ClientID%>').click();
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                    $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                    $("[id*=Targetdate]").val("");
                    $("[id*=StartTime]").val("");
                    $("[id*=Timeline]").val("");
                     $("[id*=taskDuration]").val("");
                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                    //$("[id*=Weightage]").val("");
                    $("[id*=tagto]").val("");
                    $("[id*=team]").val("");

                    $("[id*=ReminderType]").val("");
                    $("[id*=Days]").val("");
                    $("[id*=Time]").val("");
                    $("[id*=Notifyusers]").val("");
                    $("[id*=ChooseDate]").val("");

                      document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                     document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                    document.getElementById('<%=hdntasklist.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                    $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnstartdate.ClientID%>').value ="";
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = "";
                     document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "0";
                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
                    document.getElementById('<%=hdnPriority.ClientID%>').value = "";
                     document.getElementById('<%=Budget.ClientID%>').value = "";
                    document.getElementById('<%=Actual.ClientID%>').value = "";
                    document.getElementById('<%=hdnteam.ClientID%>').value = "";
                    document.getElementById('<%=hdnstage.ClientID%>').value = "";
                    document.getElementById('<%=Weightage.ClientID%>').value = "";
              document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
                    document.getElementById('<%=ChooseDate.ClientID%>').value = "";

                    document.getElementById('<%=issubtask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = 0;

                      $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
                    $(document.getElementById("<%=Notifyusers.ClientID%>")).change();

                   $(document.getElementById("<%=team.ClientID%>")).val(0);
                   $(document.getElementById("<%=team.ClientID%>")).change();

                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                        //document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                   

               
                    
                }



          function Cleardata() {
                        
                    $("[id*=ProjectId]").val("0");
                    $("[id*=Milestoneid]").val("0");
                   $("[id*=tasklist]").val("0");
                    $("[id*=taskname]").val("");
                    $("[id*=taskdescripton]").val("");
                    $("[id*=owner]").val("0");
                    $("[id*=Startdate]").val("");
                  $("[id*=Targetdate]").val("");

                 $("[id*=StartTime]").val("");
                 $("[id*=Timeline]").val("");
                 $("[id*=taskDuration]").val("");


                    $("[id*=P_Status]").val("0");
                    $("[id*=Priority]").val("0");
                    $("[id*=Reminder]").val("");
                    $("[id*=Occurrence]").val("");
                   // $("[id*=Weightage]").val("");
                    $("[id*=tagto]").val("");
                   $("[id*=team]").val("");

                   $("[id*=ReminderType]").val("");
                    $("[id*=Days]").val("");
                    $("[id*=Time]").val("");
                    $("[id*=Notifyusers]").val("");
                    $("[id*=ChooseDate]").val("");

                      document.getElementById('<%=hdnProjectId.ClientID%>').value = "0";
                     document.getElementById('<%=hdnMilestoneid.ClientID%>').value = "0";
                  document.getElementById('<%=hdntasklist.ClientID%>').value = "";

                    document.getElementById('<%=hdnowner.ClientID%>').value = "";
                     $(document.getElementById("<%=owner.ClientID%>")).val(0);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                    document.getElementById('<%=hdnstartdate.ClientID%>').value ="";
              document.getElementById('<%=hdntargetdate.ClientID%>').value = "";

              document.getElementById('<%=hdnstarttime.ClientID%>').value = "";
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = "";
                     document.getElementById('<%=taskDuration.ClientID%>').value = "0";


                 document.getElementById('<%=hdnP_Status.ClientID%>').value = "";
              document.getElementById('<%=hdnPriority.ClientID%>').value = "";
               document.getElementById('<%=Budget.ClientID%>').value = "";
                document.getElementById('<%=Actual.ClientID%>').value = "";
              document.getElementById('<%=hdnteam.ClientID%>').value = "";

              document.getElementById('<%=hdnReminderType.ClientID%>').value = "";
              document.getElementById('<%=hdnTime.ClientID%>').value = "";
              document.getElementById('<%=hdnstage.ClientID%>').value = "";
               document.getElementById('<%=Weightage.ClientID%>').value = "";
              document.getElementById('<%=hdnNotifyusers.ClientID%>').value = "";
              document.getElementById('<%=ChooseDate.ClientID%>').value = "";
              document.getElementById('<%=issubtask.ClientID%>').checked = 0;
               document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = 0;

                $(document.getElementById("<%=Notifyusers.ClientID%>")).val(0);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();

               $(document.getElementById("<%=team.ClientID%>")).val(0);
              $(document.getElementById("<%=team.ClientID%>")).change();



                   document.getElementById('<%=Button1.ClientID%>').click();
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Add Task";
         
                        
                    }

        

          function Binddetails(RecordId) {

               //alert(RecordId);
                //debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindtaskdetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1dataupdate,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
              //document.getElementById("btnreload").click();
        }


        function OnSuccess1dataupdate(response) {
          //  alert(response.d);
            //debugger
            try {
                var Bindtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnid.ClientID%>').value = Bindtaskdetail.find("id").text();
                    
                    $("[id*=ProjectId]").val(Bindtaskdetail.find("ProjectId").text());
                     $("[id*=Milestoneid]").val(Bindtaskdetail.find("Milestoneid").text());
                    $("[id*=tasklist]").val(Bindtaskdetail.find("tasklist").text());
                    $("[id*=taskname]").val(Bindtaskdetail.find("taskname").text());
                    $("[id*=taskdescripton]").val(Bindtaskdetail.find("taskdescripton").text());
                    $("[id*=owner]").val(Bindtaskdetail.find("owner").text());
                    $("[id*=Startdate]").val(Bindtaskdetail.find("Startdate").text());
                    $("[id*=Targetdate]").val(Bindtaskdetail.find("Targetdate").text()); 
                    $("[id*=P_Status]").val(Bindtaskdetail.find("P_Status").text()); 
                    $("[id*=Priority]").val(Bindtaskdetail.find("Priority").text()); 

                    $("[id*=Budget]").val(Bindtaskdetail.find("Budget").text()); 
                    $("[id*=Actual]").val(Bindtaskdetail.find("Actual").text()); 

                    $("[id*=Reminder]").val(Bindtaskdetail.find("Reminder").text()); 
                    $("[id*=Occurrence]").val(Bindtaskdetail.find("Occurrence").text()); 
                    $("[id*=tagto]").val(Bindtaskdetail.find("tagto").text()); 
                    $("[id*=team]").val(Bindtaskdetail.find("team").text()); 

                    $("[id*=ReminderType]").val(Bindtaskdetail.find("ReminderType").text()); 
                    $("[id*=Days]").val(Bindtaskdetail.find("Days").text()); 
                    $("[id*=Time]").val(Bindtaskdetail.find("team").text()); 
                    $("[id*=Notifyusers]").val(Bindtaskdetail.find("Notifyusers").text()); 
                    $("[id*=ChooseDate]").val(Bindtaskdetail.find("ChooseDate").text()); 
                    

                    document.getElementById('<%=hdnProjectId.ClientID%>').value = Bindtaskdetail.find("ProjectId").text();
                    document.getElementById('<%=hdnMilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
                    document.getElementById('<%=hdntasklist.ClientID%>').value = Bindtaskdetail.find("tasklist").text();
                    document.getElementById("divproject").style.display = "none";
                    document.getElementById("divmilestone").style.display = "none";
                    document.getElementById('<%=hdnowner.ClientID%>').value = Bindtaskdetail.find("owner").text();
                    document.getElementById('<%=hdnstartdate.ClientID%>').value = Bindtaskdetail.find("Startdate").text();
                    document.getElementById('<%=hdntargetdate.ClientID%>').value = Bindtaskdetail.find("Targetdate").text();

                    document.getElementById('<%=hdnstarttime.ClientID%>').value = Bindtaskdetail.find("StartTime").text();
                    document.getElementById('<%=hdnTimeline.ClientID%>').value = Bindtaskdetail.find("Timeline").text();
                    document.getElementById('<%=taskDuration.ClientID%>').value = Bindtaskdetail.find("TaskDuration").text();

                     document.getElementById('<%=P_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                    document.getElementById('<%=hdnP_Status.ClientID%>').value = Bindtaskdetail.find("P_Status").text();
                     document.getElementById('<%=txtnewprojectstatus.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColor.ClientID%>').style.display = "none";


                    document.getElementById('<%=Priority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                    document.getElementById('<%=hdnPriority.ClientID%>').value = Bindtaskdetail.find("Priority").text();
                     document.getElementById('<%=txtnewpriority.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolor.ClientID%>').style.display = "none";

                    document.getElementById('<%=hdnteam.ClientID%>').value = Bindtaskdetail.find("team").text();

                     document.getElementById('<%=Stage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                    document.getElementById('<%=hdnstage.ClientID%>').value = Bindtaskdetail.find("Stage").text();
                      document.getElementById('<%=txtnewstage.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtstageprintcolor.ClientID%>').style.display = "none";

                     document.getElementById('<%=Weightage.ClientID%>').value = Bindtaskdetail.find("Weightage").text();

                    document.getElementById('<%=hdnreminder.ClientID%>').value = Bindtaskdetail.find("Reminder").text();
                    document.getElementById('<%=hdnReminderType.ClientID%>').value = Bindtaskdetail.find("ReminderType").text();
                    document.getElementById('<%=hdnTime.ClientID%>').value = Bindtaskdetail.find("Time").text();
                    document.getElementById('<%=hdnNotifyusers.ClientID%>').value = Bindtaskdetail.find("Notifyusers").text();
                    document.getElementById('<%=hdnChooseDate.ClientID%>').value = Bindtaskdetail.find("ChooseDate").text();
                

                  if (Bindtaskdetail.find("issubtask").text() == 1)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("issubtask").text() == 0)
                  {
                      document.getElementById('<%=issubtask.ClientID%>').checked = false;

                 }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 1)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = true;

                  }

                  if (Bindtaskdetail.find("checkTaskWeightage").text() == 0)
                  {
                      document.getElementById('<%=checkTaskWeightage.ClientID%>').checked = false;

                  }
                  

                    // time and reminder fill and edit strt//

                     if (Bindtaskdetail.find("Reminder").text() == "0")
                     {
                       document.getElementById("divremindertype").style.display = "none";
                       document.getElementById("divDays").style.display = "none";
                       document.getElementById("Divtime").style.display = "none";
                       document.getElementById("divNotifyusers").style.display = "none";
                       document.getElementById("divChooseDate").style.display = "none";

                    }


                     //  Milestone Timeline //
              //debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
               //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }



             //  Task Timeline //
             //debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = Bindtaskdetail.find("Milestoneid").text();
             //alert(projec_id);
        
                //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }


                    GetReminderDtls();
                    GetRemindertypeDtls();
                    // time and reminder fill and edit end//


             if (Bindtaskdetail.find("Notifyusers").text() != "")
             {
             var idsemp = Bindtaskdetail.find("Notifyusers").text().split(',');
            $(document.getElementById("<%=Notifyusers.ClientID%>")).val(idsemp);
             $(document.getElementById("<%=Notifyusers.ClientID%>")).change();
            }
           document.getElementById('<%=Button1.ClientID%>').click();

             GetOwnerempdetails();
             if (Bindtaskdetail.find("owner").text() != "")
              {
                  //debugger
                  if (Bindtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
                  else
                  {

                   var idsemp = Bindtaskdetail.find("owner").text();
                  $(document.getElementById("<%=owner.ClientID%>")).val(idsemp);
                  $(document.getElementById("<%=owner.ClientID%>")).change();

                  }
             
             }



             GetTeamempdetails();
             if (Bindtaskdetail.find("team").text() != "")
             {
                 //debugger
                 if (Bindtaskdetail.find("team").text().includes(','))
                 {
                      var idsemp = Bindtaskdetail.find("team").text().split(',');
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = Bindtaskdetail.find("team").text();
                     $(document.getElementById("<%=team.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=team.ClientID%>")).change();
                 }
                 

            }


                     document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('btnsave').value = "Update";
                   document.getElementById('<%=lbltitle.ClientID%>').innerHTML= "Edit  Task";
                }
            }
            catch
                {
                }
            
            }


        
        
        function  updatesrc(strpid,strmid,strtid,imgtype,ctrlid)
        {
            //alert('ok');
            //debugger
            var ids = ctrlid.split('_')[1];
       var img = document.getElementById("imgid"+imgtype+"_"+ids);
        var currentSrc = img.src;

        if (currentSrc.includes('open.png')) {
      toggleHiddenFieldValue(strpid,strmid,strtid,'close');
            img.src = '/pms/assets/images/close.png';
            img.alt = 'close';
        } else {
         toggleHiddenFieldValue(strpid,strmid,strtid,'open');
            img.src = '/pms/assets/images/open.png';
            img.alt = 'open';

        }



            
//$('.timesheet-wrap').daterangepicker({
  //  opens: 'center'
  //});

//$('.dayswrapper').daterangepicker({
  //  opens: 'center',
  //  locale: {
   //   format: 'MM/DD/YYYY'
    //}
 // });

        }

            </script>

       

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<asp:HiddenField ID="hdnbtns" runat="server" />
    <script>
        
        $(document).ready(function () {
            

            Bindtask_data();

          
           
});
        function reloaddata() {
            Bindtask_data();
            viewalldetails();
        }

         function Bindtask_data() {
     
             //alert($("[id*=hdiprojid]").val());
            //debugger
                  $.ajax({
                    url: 'makemilestone.aspx/bindtaskdata',                  
                    //data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    data: "{ 'RecordId': '" + $("[id*=hdiprojid]").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetailsmain,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        //fixedColumns: {
        //    leftColumns: 4
        //}
        function OnSuccessgetdetailsmain(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblprojectdata.ClientID%>");
                    lbl.innerHTML = '';                    
            lbl.innerHTML = todolist;
      
                table = new DataTable('.datatable', {
                    scrollX: true,
                    scrollY: 500,
                    scrollCollapse: true,
                    paging: false,
                    searching: true,
ordering: false,autoWidth: false
                     
      

                });
     

         <%=setmilstone%>


           


            hidecolumngrid();
initializedatepker();


        }
      

    </script>



<script>
    
    $(document).ready(function ()
    {
            Bindmilestonetimeline_data();
});
    function Bindmilestonetimeline_data()
    {
             ////debugger
            
             
             var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);


                //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           
        }

    </script>

<script>
    
    $(document).ready(function ()
    {
            Bindtasktimeline_data();
});
    function Bindtasktimeline_data()
    {
             //debugger
            
             
             var mile_id = document.getElementById("<%=Milestoneid.ClientID%>").value;
             //alert(projec_id);


                //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccesstask_timeline(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
           try {
    //debugger;

    // Dispose existing tooltips
    const oldTooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    oldTooltips.forEach(function (el) {
        const tooltip = bootstrap.Tooltip.getInstance(el);
        if (tooltip) {
            tooltip.dispose();
        }
    });

    // Select elements with data-bs-toggle and update attributes
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(function (el) {
        el.setAttribute('data-bs-placement', 'top');
        el.setAttribute('data-bs-trigger', 'hover');
    });

    // Initialize new tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl, {
            placement: 'top',
            trigger: 'hover'
        });
    });

} catch (e) {
    console.error('Tooltip init error:', e);
}

        }

    </script>

    <script>

        function deletedata(strpid,strmid,RecordId,btn) {
            

                 if (confirm('Do you really want to delete the Entery?'))
                 {
                     //toggleHiddenFieldValue(strpid, strmid, RecordId,'delete'); 
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'MultiProjectTask.aspx/Deletetaskdata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    //success: OnSuccessdelete,
                    success: function (response)
                    {
//debugger
                    alert('Data Deleted !');
                      //const row = btn.closest('tr');
                      //row.remove();
                    Bindtask_data();
                    //viewtask(strpid,strmid);
                  viewalldetails();
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

        function OnSuccessdelete(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>

  

    <%--save data end  --%> 


 <%--Owner details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeDataowner(val12A) {

            if (Page_ClientValidate("btnsaveowner")) {
               
                //debugger
                
                // save all details

            
                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/Savetask_Owner',
                    //data: '{Bindtaskdetail: ' + JSON.stringify(Bindtaskdetail) + '}',
                     data: "{ 'id': '" + document.getElementById('<%=hdnchangeid.ClientID%>').value + "' ,'ownerdropdownid': '" + document.getElementById('<%=hdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessowner,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessowner(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddlchangeowner]").val("");
                   
                     document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
                    document.getElementById("btnclsowner").click();
                    // Bindproject_data();
                    //debugger
                    
                    document.getElementById("lblowner_" + document.getElementById("<%=hdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
          
        
                    
                }

        
 
          function Binddetails_Owner(RecordId,lblownerid) {
              
              //debugger;

               document.getElementById("<%=hdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindtaskdetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1owner,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1owner(response) {
            
            //debugger
            try {
                var Bindtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnchangeid.ClientID%>').value = Bindtaskdetail.find("id").text();
                    document.getElementById('<%=hdncahngeowner.ClientID%>').value = Bindtaskdetail.find("owner").text();

           
             document.getElementById('<%=multichngmilestoneid.ClientID%>').value = Bindtaskdetail.find("Milestoneid").text();
              Multichangeowner();
              if (Bindtaskdetail.find("owner").text() != "")
              {
                  //debugger
                  if (Bindtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindtaskdetail.find("owner").text();
                $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change();
                  }
             
            }


             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    <script>
function removemileowner(reordid, ownerid) {

    debugger;
//alert("ok");
    var projectowneres = {
        id: reordid,
        Owner: ownerid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removemileownerbyid',
        data: JSON.stringify({ projectowneres:projectowneres }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("milelblowner_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}
function removemileteam(reordid, teamid) {

    debugger;
//alert("ok");
    var projectteams = {
        id: reordid,
        Team: teamid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removemileteambyid',
        data: JSON.stringify({ projectteams: projectteams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdnaddteam.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("milelbluser_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}

function removetaskowner(reordid, ownerid) {

    debugger;
//alert("ok");
    var projectowneres = {
        id: reordid,
        Owner: ownerid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removetaskownerbyid',
        data: JSON.stringify({ projectowneres: projectowneres }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("lblowner_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}
function removetaskteam(reordid, teamid) {

    debugger;
//alert("ok");
    var projectteams = {
        id: reordid,
        Team: teamid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removetaskteambyid',
        data: JSON.stringify({ projectteams: projectteams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdnaddteam.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("lbluser_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}
function removesubtaskowner(reordid, ownerid) {

    debugger;
//alert("ok");
    var projectowneres = {
        id: reordid,
        Owner: ownerid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removesubtaskownerbyid',
        data: JSON.stringify({ projectowneres: projectowneres }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdncahngeowner.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("sbtsklblowner_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}
function removesubtaskteam(reordid, teamid) {

    debugger;
//alert("ok");
    var projectteams = {
        id: reordid,
        Team: teamid
    };

    $.ajax({
        type: "POST",
        url: 'makemilestone.aspx/removesubtaskteambyid',
        data: JSON.stringify({ projectteams: projectteams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            debugger;

            document.getElementById('<%=hdnaddteam.ClientID%>').value = "";

            if (response && response.d) {
                document.getElementById("sbtsklbluser_" + reordid).innerHTML = response.d;
            }
        },

        error: function (response) {
            debugger;
            alert("Error: " + response.responseText);
        }
    });

    return false;
}

    </script>
      <%--Owner details bind and update data end  --%>



<%--Team details bind and update data start --%>

    <script type="text/javascript">
             
        function SvaeDatateam(val13A) {

            if (Page_ClientValidate("btnsaveteam")) {
               
                //debugger


                // save all details

                var taskdetails = {};
                taskdetails.id = document.getElementById('<%=hdnteameid.ClientID%>').value;;
                taskdetails.team = document.getElementById('<%=hdnaddteam.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/Save_Team',
                    data: '{taskdetails: ' + JSON.stringify(taskdetails) + '}',
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeam,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessTeam(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=ddladdteam]").val("");
                   
                     document.getElementById('<%=hdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=ddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=ddladdteam.ClientID%>")).change();
                    document.getElementById("btnclsTeam").click();
                   // Bindtask_data();
                   document.getElementById("lbluser_" + document.getElementById("<%=hdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        
 
          function Binddetails_Team(RecordId,lbluserid) {

          
              //debugger;
               document.getElementById("<%=hdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1team,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1team(response) {
            
            //debugger
            try {
                var taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnteameid.ClientID%>').value = taskdetails.find("id").text();
                    document.getElementById('<%=hdnaddteam.ClientID%>').value = taskdetails.find("team").text();

              
              document.getElementById('<%=multichngmilestoneid.ClientID%>').value = taskdetails.find("Milestoneid").text();
              MultichangeTeam();
              if (taskdetails.find("team").text() != "")
              {
                  //debugger
                  if (taskdetails.find("team").text().includes(','))
                  {
                      var idsemp = taskdetails.find("team").text().split(',');
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = taskdetails.find("team").text();
                $(document.getElementById("<%=ddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=ddladdteam.ClientID%>")).change();
                  }
             
            }



             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>

    <%--Team details bind and update data end  --%>


 <%--status and propery  update start--%>
    <asp:HiddenField ID="hdtaskid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStatusdeatils(statusid,iscomplete)
        {
            //debugger
                   var taskid=document.getElementById("<%=hdtaskid.ClientID%>").value;
                    //alert(iscomplete);
                    $.ajax({
                    url: '/pms/MultiProjectTask.aspx/Update_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'statusid': '" + statusid + "' ,'iscomplete': '" + iscomplete + "' }",
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
            if (response.d.includes("~~")) {
               // alert('Data Saved successfully.');
                //Bindtask_data();
                var btnid = document.getElementById("<%=hdnbtnstatusid.ClientID%>").value;
                var arrcol = response.d.split("~~");
                document.getElementById(btnid).innerHTML = arrcol[0];
                document.getElementById(btnid).style.backgroundColor = arrcol[1];
//viewalldetails();
            }
            else {
                alert(response.d);
            }
        }
        
        function setprojectid(taskid,btnststusid)
        {
            //debugger
             document.getElementById("<%=hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdtaskid.ClientID%>").value = taskid;
        }
            </script>


      <asp:HiddenField ID="hdnpropertID" runat="server" Value="0" />
<asp:HiddenField ID="hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDatapropertydeatils(Propertyid)
        {
                   //debugger
                   var taskid=document.getElementById("<%=hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/UpdatePriority_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt(response) {
            //alert('Data Saved successfully.');
            //Bindtask_data();
             var btnid = document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function setpriorityid(tasklistid,btnpriortyid)
        {
            //debugger
            document.getElementById("<%=hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=hdnpropertID.ClientID%>").value = tasklistid;
        }
            </script>

    <%--status and  proprety update end--%>



<%--Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeExtended_task(val225A) {

            if (Page_ClientValidate("btnsaveextended_task")) {
               
                //debugger
                
                // save all details


                var task_id = document.getElementById('<%=hdnextendid_task.ClientID%>').value;
                var extendStrtdate_task = document.getElementById('<%=Extendatestrt_task.ClientID%>').value;
                var extendEnddate_task = document.getElementById('<%=Extendateend_task.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/UpdateExtendeddatedtls_task',
                    data: "{ 'task_id': '" + task_id + "' ,'extendStrtdate_task': '" + extendStrtdate_task + "','extendEnddate_task': '" + extendEnddate_task + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdtexdate_task,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessupdtexdate_task(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=hdnextndstrt_task.ClientID%>').value = "";
                    document.getElementById('<%=hdnextendid_task.ClientID%>').value = "";
                    document.getElementById("btnclsextended_task").click();
                    //Bindproject_data();
                    //debugger
                    
                   
                    document.getElementById("lblExtnddttask_" + document.getElementById("<%=hdnextnhtml_task.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Bindextenddatetask_Details(RecordId,lblExtndid) {
              
              //debugger;

              document.getElementById("<%=hdnextnhtml_task.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindtaskdetail_extendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_task,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_task(response) {
            // alert(response.d);
            //debugger
            try {
                var taskdetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (taskdetails.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid_task.ClientID%>').value = taskdetails.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt_task.ClientID%>').value = taskdetails.find("StartDate").text();
                    document.getElementById('<%=hdnextndend_task.ClientID%>').value = taskdetails.find("TargetDate").text();
                     document.getElementById('<%=Button4.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }

         

            </script>
    
      <%--Extended date details bind and update data end  --%>



<%--All stage update start--%>

 <asp:HiddenField ID="hdntaskstageid" runat="server" Value="0" />
 <asp:HiddenField ID="hdnbtn_stageid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function UpdateDataStagedeatils(Stageid)
        {
            //debugger
                   var taskid=document.getElementById("<%=hdntaskstageid.ClientID%>").value;
                    //alert(taskid);
                    //alert(Stageid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/Update_Stagge',
                    data: "{ 'taskid': '" + taskid + "' ,'Stageid': '" + Stageid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStageupdt,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStageupdt(response) {
            alert('Data Saved successfully.');
           // Bindtask_data();
            var btnstgid = document.getElementById("<%=hdnbtn_stageid.ClientID%>").value; 
            var arrstgcol = response.d.split("~~");
            document.getElementById(btnstgid).innerHTML = arrstgcol[0];
            document.getElementById(btnstgid).style.backgroundColor = arrstgcol[1];
        }
        
        function setStageid(taskid,btnStageid)
        {
            //debugger
             document.getElementById("<%=hdnbtn_stageid.ClientID%>").value = btnStageid;
            document.getElementById("<%=hdntaskstageid.ClientID%>").value = taskid;
        }
            </script>

<%--All stage update end--%>



    
<asp:HiddenField ID="hdnprojecttaskid" runat="server" Value="0" />

<script>
    function initializeBootstrapTooltips() {
        // Bootstrap 5 tooltip reinitialization
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Initial page load
    document.addEventListener('DOMContentLoaded', function () {
        initializeBootstrapTooltips();
    });

    // After ASP.NET UpdatePanel postback
    Sys.Application.add_load(function () {
        initializeBootstrapTooltips();
    });
</script>

<script type="text/javascript" language="javascript">
        function isnumeric(event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Please enter only Number value');
                return false;
            }
            else {
                //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
                var parts = event.srcElement.value.split('.');
                if (parts.length > 1 && charCode == 46) {
                    return false;
                }
                return true;
            }
        }
    </script>
 <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
    
<script src="/pms/assets/js/select2.min.js"></script> 
     <%--Multi dropdown select start --%> 

 <%--<script>
    //debugger
    var ddlowner = $(document.getelementbyid("<%=owner.clientid%>"));
     $(ddlowner).select2({
        data: [
    <%=multiowner%>
        ],
        multiple: true,
        placeholder: "search owner",
        width: "100%"
    });

   
     $(document.getelementbyid("<%=owner.clientid%>")).change(function () {
        //debugger
        var ids = $(document.getelementbyid("<%=owner.clientid%>")).val();
        document.getelementbyid("<%=hdnowner.clientid %>").value = ids;
        document.getelementbyid("<%=hdnowner.clientid%>").click();

    });
   
  
    </script>--%>

  <%--<script>
    //debugger
    var ddteam = $(document.getelementbyid("<%=team.clientid%>"));
   

    $(ddteam).select2({
        data: [
    <%=multiteam%>
        ],
        multiple: true,
        placeholder: "search team",
        width: "100%"
    });

   
     $(document.getelementbyid("<%=team.clientid%>")).change(function () {
        //debugger
                                                    var ids = $(document.getelementbyid("<%=team.clientid%>")).val();
        document.getelementbyid("<%=hdnteam.clientid %>").value = ids;
        document.getelementbyid("<%=team.clientid%>").click();

    });
   
  
    </script>--%>

  <script src="/pms/assets/js/jscolor.js"></script>
<script src="/pms/assets/js/jquery.dataTables.min.js"></script>
                    <script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>
<script>
    //debugger
    var ddLNotifyusers = $(document.getElementById("<%=Notifyusers.ClientID%>"));
   

    $(ddLNotifyusers).select2({
        data: [
    <%=Multiusers%>
        ],
        multiple: true,
        placeholder: "Search Notify users",
        width: "100%"
    });

   
     $(document.getElementById("<%=Notifyusers.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=Notifyusers.ClientID%>")).val();
        document.getElementById("<%=hdnNotifyusers.ClientID %>").value = ids;
        document.getElementById("<%=Notifyusers.ClientID%>").click();

    });
   
  
    </script>

 <asp:HiddenField ID="multichngmilestoneid" runat="server" Value="0" />
<script>
    <%--//debugger
    var ddLchangeowner = $(document.getElementById("<%=ddlchangeowner.ClientID%>"));
   

    $(ddLchangeowner).select2({
        data: [
    <%=Multichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#changeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=hdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=ddlchangeowner.ClientID%>").click();

    });--%>



      function Multichangeowner() {
        //debugger
        var mile_stoneid = document.getElementById("<%=multichngmilestoneid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLchangeowner = $(document.getElementById("<%=ddlchangeowner.ClientID%>"));
                                                         $(ddLchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#changeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ddlchangeowner.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=ddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=hdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=ddlchangeowner.ClientID%>").click();

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
   
  
    </script>

<script>
    <%--//debugger
    var ddLAddteam = $(document.getElementById("<%=ddladdteam.ClientID%>"));
   

    $(ddLAddteam).select2({
        data: [
    <%=Multiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#addteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=ddladdteam.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=ddladdteam.ClientID%>")).val();
        document.getElementById("<%=hdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=ddladdteam.ClientID%>").click();

    });--%>


    
       function MultichangeTeam() {
        //debugger
        var mile_stoneid = document.getElementById("<%=multichngmilestoneid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLAddteam = $(document.getElementById("<%=ddladdteam.ClientID%>"));
                                                         $(ddLAddteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#addteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=ddladdteam.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=ddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=hdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=ddladdteam.ClientID%>").click();

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


   
  
    </script>





    <%--Multi dropdown select start end--%>   




 <%--Milestone Data show--%>   

<%--Milestone Owner details bind and update data start --%>
<asp:HiddenField ID="multichngprojectidmile" runat="server" Value="0" />
<script>
  

   <%--  //debugger
    var mileddlchangeowner = $(document.getElementById("<%=mileddlchangeowner.ClientID%>"));
   

    $(mileddlchangeowner).select2({
        data: [
    <%=mileMultichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#milechangeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=milehdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=mileddlchangeowner.ClientID%>").click();

    });--%>


    function MultichangeownerMile() {
        //debugger
        var projectid = document.getElementById("<%=multichngprojectidmile.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeOwnerMile_new',
            data: "{'projectid': '" + projectid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var mileddlchangeowner = $(document.getElementById("<%=mileddlchangeowner.ClientID%>"));
                                                         $(mileddlchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#milechangeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=milehdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=mileddlchangeowner.ClientID%>").click();

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





   
  
    </script>
<script type="text/javascript">
             
        function mileSvaeDataowner(val12A) {

            if (Page_ClientValidate("milebtnsaveowner")) {
               
                //debugger


                // save all details
                
                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/SaveMilestone_Owner',
                    data: "{ 'mileid': '" + document.getElementById('<%=milehdnchangeid.ClientID%>').value + "' ,'mileownerdropdownid': '" + document.getElementById('<%=milehdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessowner_mile,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessowner_mile(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=mileddlchangeowner]").val("");
                   
                     document.getElementById('<%=milehdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();
                    document.getElementById("milebtnclsowner").click();
                   // Bindmilestone_data();
                     document.getElementById("milelblowner_" + document.getElementById("<%=milehdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function mile_Binddetails_Owner(RecordId,lblownerid) {
              
              debugger;
              document.getElementById("<%=milehdnchnownrghtml.ClientID%>").value = lblownerid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/BindpMilestonedetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1owner_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1owner_mile(response) {
            
            debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnchangeid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=milehdncahngeowner.ClientID%>').value = milestonedetails.find("Owner").text();

             document.getElementById('<%=multichngprojectidmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
             MultichangeownerMile();
              if (milestonedetails.find("Owner").text() != "")
              {
                  //debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=mileddlchangeowner.ClientID%>")).change();
                  }
             
            }

             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
<%--Milestone Owner details bind and update data end  --%>


<%--Milestone Team details bind and update data start --%>
<script>
   <%-- //debugger
    var mileddLAddteam = $(document.getElementById("<%=mileddladdteam.ClientID%>"));
   

    $(mileddLAddteam).select2({
        data: [
    <%=mileMultiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#mileaddteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=mileddladdteam.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=mileddladdteam.ClientID%>")).val();
        document.getElementById("<%=milehdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=mileddladdteam.ClientID%>").click();

    });--%>



     function MultichangeTeamMile() {
        //debugger
        var projectid = document.getElementById("<%=multichngprojectidmile.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeTeamMile_new',
            data: "{'projectid': '" + projectid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var mileddLAddteam = $(document.getElementById("<%=mileddladdteam.ClientID%>"));
                                                         $(mileddLAddteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#mileaddteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=mileddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=milehdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=mileddladdteam.ClientID%>").click();

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
   
  
    </script>
<script type="text/javascript">
             
        function mileSvaeDatateam(val13A) {

            if (Page_ClientValidate("milebtnsaveteam")) {
               
                //debugger


                // save all details
                
                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/SaveMileProject_Team',
                    data: "{ 'mileid': '" + document.getElementById('<%=milehdnteameid.ClientID%>').value + "' ,'mileTeamdropdownid': '" + document.getElementById('<%=milehdnaddteam.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeam_mile,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessTeam_mile(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=mileddladdteam]").val("");
                   
                     document.getElementById('<%=milehdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();
                    document.getElementById("milebtnclsTeam").click();
                  //  Bindmilestone_data();
                   document.getElementById("milelbluser_" + document.getElementById("<%=milehdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                    
                }

        

          function mileBinddetails_Team(RecordId,lbluserid) {
              
              //debugger;
              //alert(RecordId);
              //alert(lbluserid);
               document.getElementById("<%=milehdnaddteamhtml.ClientID%>").value = lbluserid.split('_')[1];
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindmilestonedetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1team_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1team_mile(response) {
            
            //debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnteameid.ClientID%>').value = milestonedetails.find("id").text();
                    document.getElementById('<%=milehdnaddteam.ClientID%>').value = milestonedetails.find("team").text();
                    
              document.getElementById('<%=multichngprojectidmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
              MultichangeTeamMile();
              if (milestonedetails.find("team").text() != "")
              {
                  //debugger
                  if (milestonedetails.find("team").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("team").text().split(',');
                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("team").text();
                $(document.getElementById("<%=mileddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=mileddladdteam.ClientID%>")).change();
                  }
             
            }

             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>

<%--Milestone Team details bind and update data end --%>



<%--Milestone  status update start--%>
    <asp:HiddenField ID="hdnmilstntid" runat="server" Value="0" />
 <asp:HiddenField ID="milehdnbtnstatusid" runat="server" Value="0" />

    <script type="text/javascript">
             
        function MileUpdateDataStatusdeatils(statusid,iscomplete)
        {
            //debugger
            var milstoneid=document.getElementById("<%=hdnmilstntid.ClientID%>").value;
                    //alert(projectid);url: 'makemilestone.aspx/UpdateMilestone_Status',
                    $.ajax({
                    url: '/pms/MultiProjectTask.aspx/UpdateMilestone_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdt_mile(response) {
             
            if (response.d.includes("~~")) {
                var arrcol = response.d.split("~~");
                //alert('Data Saved successfully.');
               //Bindtask_data();
               var btnid = document.getElementById("<%=milehdnbtnstatusid.ClientID%>").value;
                document.getElementById(btnid).innerHTML = arrcol[0];
                document.getElementById(btnid).style.backgroundColor = arrcol[1];
//viewalldetails();
            }
            else {
                alert(response.d);
            }
            
        }
        
        function setmilsestoneid(milstoneid,btnststusid)
        {
            //debugger
            document.getElementById("<%=milehdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdnmilstntid.ClientID%>").value = milstoneid;
        }
            </script>

 <%-- Milestone  status update end --%>


   <%--Milestone property update start--%>

      <asp:HiddenField ID="milehdnpropertID" runat="server" Value="0" />
      <asp:HiddenField ID="milehdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function MileUpdateDatapropertydeatils(Propertyid)
        {
                   //debugger
                   var milstoneid=document.getElementById("<%=milehdnpropertID.ClientID%>").value;
                   $.ajax({
                    url: 'MultiProjectTask.aspx/MileUpdatePriority_Status',
                    data: "{ 'milstoneid': '" + milstoneid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt_mile(response) {
            //alert('Data Saved successfully.');
           // Bindmilestone_data();
            var btnid = document.getElementById("<%=milehdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function milesetpriorityid(milston_id,btnpriortyid)
        {
            //debugger
             document.getElementById("<%=milehdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=milehdnpropertID.ClientID%>").value = milston_id;
        }
            </script>

    <%--Milestone proprety update end--%>


 <%--Milestone Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function mileSvaeExtended(val125A) {

            if (Page_ClientValidate("milebtnsaveextended")) {
               
                //debugger
                
                // save all details


                var Milestoneid = document.getElementById('<%=milehdnextendid.ClientID%>').value;
                var mileextendStrtdate = document.getElementById('<%=mileExtendatestrt.ClientID%>').value;
                var mileextendEnddate = document.getElementById('<%=mileExtendateend.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/MileUpdateExtendeddatedtls',
                    data: "{ 'Milestoneid': '" + Milestoneid + "' ,'mileextendStrtdate': '" + mileextendStrtdate + "','mileextendEnddate': '" + mileextendEnddate + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessUpdateexdate_mile,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessUpdateexdate_mile(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=milehdnextndstrt.ClientID%>').value = "";
                    document.getElementById('<%=milehdnextendid.ClientID%>').value = "";
                    document.getElementById("milebtnclsextended").click();
                    //debugger
                    document.getElementById("milelblExtnddt_" + document.getElementById("<%=milehdnextnhtml.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function MileBindextenddate_Details(RecordId,lblExtndid) {
              
              //debugger;

              document.getElementById("<%=milehdnextnhtml.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindprojectdetail_Mileextendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_mile(response) {
            // alert(response.d);
            //debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=milehdnextendid.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=milehdnextndstrt.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=milehdnextndend.ClientID%>').value = milestonedetails.find("EndDate").text();
                     document.getElementById('<%=Button2.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }
         

            </script>
    
      <%--Milestone Extended date details bind and update data end  --%>


  <%--Project status update start--%>
    <asp:HiddenField ID="proj_hdnprojectid" runat="server" Value="0" />
    <asp:HiddenField ID="proj_hdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function ProjUpdateDataStatusdeatils(statusid,iscomplete)
        {
            //debugger
            var projectid=document.getElementById("<%=proj_hdnprojectid.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/UpdateProject_Status',
                    //data: "{ 'RecordId': '" + RecordId + "'}",
                    data: "{ 'projectid': '" + projectid + "' ,'statusid': '" + statusid + "' ,'iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStatusupdt_project,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStatusupdt_project(response) {
            //debugger
            alert('Data Saved successfully.');
            var btnid = document.getElementById("<%=proj_hdnbtnstatusid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
            //btnid.innerHTML = response.d;
            Bindtask_data();
        }
        
        function projectsetprojectid(projectid,btnststusid)
        {
            //debugger
            //var id = btnststusid;
            document.getElementById("<%=proj_hdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=proj_hdnprojectid.ClientID%>").value = projectid;
        }
            </script>

 <%--   status update end --%>


   <%--Project property update start--%>

      <asp:HiddenField ID="proj_hdnpropertID" runat="server" Value="0" />
      <asp:HiddenField ID="proj_hdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function ProjUpdateDatapropertydeatils(Propertyid)
        {
                   //debugger
                   var projectid=document.getElementById("<%=proj_hdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/projectUpdatePriority_Status',
                    data: "{ 'projectid': '" + projectid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdt_project,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdt_project(response) {
            alert('Data Saved successfully.');
            var btnid = document.getElementById("<%=proj_hdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
           // Bindproject_data();
        }
        
        function projectsetpriorityid(projectid,btnpriortyid)
        {
            //debugger
            document.getElementById("<%=proj_hdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=proj_hdnpropertID.ClientID%>").value = projectid;
        }
            </script>

    <%--Project proprety update end--%>



 <%--Multi sub task details start--%>

 <%--Extended date details bind and update data start --%>

     <script type="text/javascript">
             
        function SvaeExtended_subtask(val225A) {

            if (Page_ClientValidate("btnsaveextended_subtask")) {
               
                //debugger
                
                // save all details


                var subtask_id = document.getElementById('<%=hdnextendid_subtask.ClientID%>').value;
                var extendStrtdate_subtask = document.getElementById('<%=Extendatestrt_subtask.ClientID%>').value;
                var extendEnddate_subtask = document.getElementById('<%=Extendateend_subtask.ClientID%>').value;
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/UpdateExtendeddatedtls_subtask',
                    data: "{ 'subtask_id': '" + subtask_id + "' ,'extendStrtdate_subtask': '" + extendStrtdate_subtask + "','extendEnddate_subtask': '" + extendEnddate_subtask + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessupdtexdate_subtask,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessupdtexdate_subtask(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    document.getElementById('<%=hdnextndstrt_subtask.ClientID%>').value = "";
                    document.getElementById('<%=hdnextendid_subtask.ClientID%>').value = "";
                    document.getElementById("btnclsextended_subtask").click();
                    //Bindproject_data();
                    //debugger
                    
                   
                    document.getElementById("lblExtnddtsubtask_" + document.getElementById("<%=hdnextnhtml_subtask.ClientID%>").value).innerHTML =  response.d;
         
                  
                }

        

          function Bindextenddatesubtask_Details(RecordId,lblExtndid) {
              
              //debugger;

              document.getElementById("<%=hdnextnhtml_subtask.ClientID%>").value = lblExtndid.split('_')[1];
              
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindsubtaskdetail_extendatedate',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessExtendate_subtask,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccessExtendate_subtask(response) {
            // alert(response.d);
            //debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=hdnextendid_subtask.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    
                    document.getElementById('<%=hdnextndstrt_subtask.ClientID%>').value = Bindsubtaskdetail.find("StartDate").text();
                    document.getElementById('<%=hdnextndend_subtask.ClientID%>').value = Bindsubtaskdetail.find("TargetDate").text();
                     document.getElementById('<%=Button5.ClientID%>').click();
             
                   
                }
            }
            catch
                {
                }

            }

         

            </script>
    
      <%--Extended date details bind and update data end  --%>



 <%--status and propery  update start--%>
    <asp:HiddenField ID="hdsubtaskid" runat="server" Value="0" />
 <asp:HiddenField ID="sbtskhdnbtnstatusid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDataStatusdeatils(statusid,iscomplete)
        {
            debugger
                   var subtaskid=document.getElementById("<%=hdsubtaskid.ClientID%>").value;
                     //alert(statusid);
                    //alert(iscomplete);makemilestone
                    $.ajax({
                    url: '/pms/MultiProjectTask.aspx/sbtskUpdate_Status',
                    data: "{ 'subtaskid': '" + subtaskid + "' ,'statusid': '" + statusid + "','iscomplete': '" + iscomplete + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function(response)
{
 if (response.d.includes("~~")) {
        alert('Data Saved successfully.');

        var btnid = document.getElementById("<%=sbtskhdnbtnstatusid.ClientID%>").value;
        var arrcol = response.d.split("~~");

       document.getElementById(btnid).innerHTML = arrcol[0];
        document.getElementById(btnid).style.backgroundColor = arrcol[1];

if(iscomplete=="1")
{
        var dvid = btnid.split("_")[1];
        var progElement = document.getElementById('dvprogsub_' + dvid);

        progElement.classList.remove('bg-warning');
        progElement.classList.add('bg-success');

        progElement.classList.add('progress-bar');
}

        
    } else {
        alert(response.d);
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

        function OnSuccessStatusupdtsbtsk(response) {
    debugger;

    if (response.d.includes("~~")) {
        alert('Data Saved successfully.');

        var btnid = document.getElementById("<%=sbtskhdnbtnstatusid.ClientID%>").value;
        var arrcol = response.d.split("~~");

       document.getElementById(btnid).innerHTML = arrcol[0];
        document.getElementById(btnid).style.backgroundColor = arrcol[1];

        var dvid = btnid.split("_")[1];
        var progElement = document.getElementById('dvprogsub_' + dvid);

        progElement.classList.remove('bg-warning');
        progElement.classList.add('bg-success');

        progElement.classList.add('progress-bar');

        
    } else {
        alert(response.d);
    }
}

        
        function setsubtaskstustid(subtaskid,btnststusid)
        {
            //debugger
            document.getElementById("<%=sbtskhdnbtnstatusid.ClientID%>").value = btnststusid;
            document.getElementById("<%=hdsubtaskid.ClientID%>").value = subtaskid;
        }
            </script>


      <asp:HiddenField ID="sbtskhdnpropertID" runat="server" Value="0" />
<asp:HiddenField ID="sbtskhdnbtnpriortyid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDatapropertydeatils(Propertyid)
        {
                   //debugger
                   var taskid=document.getElementById("<%=sbtskhdnpropertID.ClientID%>").value;
                    //alert(projectid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskUpdatePriority_Status',
                    data: "{ 'taskid': '" + taskid + "' ,'Propertyid': '" + Propertyid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessPriorityupdtsbtsk2,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessPriorityupdtsbtsk2(response) {
            alert('Data Saved successfully.');
           // BindSubtask_data();
            var btnid = document.getElementById("<%=sbtskhdnbtnpriortyid.ClientID%>").value; 
            var arrcol = response.d.split("~~");
            document.getElementById(btnid).innerHTML = arrcol[0];
            document.getElementById(btnid).style.backgroundColor = arrcol[1];
        }
        
        function sbtsksetpriorityid(tasklistid,btnpriortyid)
        {
            //debugger
            document.getElementById("<%=sbtskhdnbtnpriortyid.ClientID%>").value = btnpriortyid;
            document.getElementById("<%=sbtskhdnpropertID.ClientID%>").value = tasklistid;
        }
            </script>

    <%--status and  proprety update end--%>
    

<%--All stage update start--%>

 <asp:HiddenField ID="sbtskhdntaskstageid" runat="server" Value="0" />
 <asp:HiddenField ID="sbtskhdnbtn_stageid" runat="server" Value="0" />
    <script type="text/javascript">
             
        function sbtskUpdateDataStagedeatils(Stageid)
        {
            //debugger
                   var sbtsktaskid=document.getElementById("<%=sbtskhdntaskstageid.ClientID%>").value;
                    //alert(taskid);
                    //alert(Stageid);
                    $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskUpdate_Stagge',
                    data: "{ 'taskid': '" + sbtsktaskid + "' ,'Stageid': '" + Stageid + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessStageupdtsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }

        function OnSuccessStageupdtsbtsk(response) {
            alert('Data Saved successfully.');
           // Bindtask_data();
            var btnstgid = document.getElementById("<%=sbtskhdnbtn_stageid.ClientID%>").value; 
            var arrstgcol = response.d.split("~~");
            document.getElementById(btnstgid).innerHTML = arrstgcol[0];
            document.getElementById(btnstgid).style.backgroundColor = arrstgcol[1];
        }
        
        function sbtsksetStageid(sbtsktaskid,btnStageid)
        {
            //debugger
             document.getElementById("<%=sbtskhdnbtn_stageid.ClientID%>").value = btnStageid;
            document.getElementById("<%=sbtskhdntaskstageid.ClientID%>").value = sbtsktaskid;
        }
            </script>

<%--All stage update end--%>



 <%--Owner details bind and update data start --%>

<asp:HiddenField ID="multichngtaskid" runat="server" Value="0" />
<script>
   <%-- //debugger
    var sbtskddLchangeowner = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>"));
   

    $(sbtskddlchangeowner).select2({
        data: [
    <%=sbtskMultichangeowner%>
        ],
        multiple: true,
        placeholder: "Search Owner",
        dropdownParent: $("#sbtskchangeowner"),
        width: "100%"
    });

   
     $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val();
        document.getElementById("<%=sbtskhdncahngeowner.ClientID %>").value = ids;
        document.getElementById("<%=sbtskddlchangeowner.ClientID%>").click();

    });--%>


      function Multichangeownersubtask() {
        //debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangeOwnersubtask_new',
            data: "{'tsk_id': '" + tsk_id + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var sbtskddlchangeowner = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>"));
                                                         $(sbtskddlchangeowner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             dropdownParent: $("#sbtskchangeowner"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val();
                                                        document.getElementById("<%=sbtskhdncahngeowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=sbtskddlchangeowner.ClientID%>").click();

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


   
  
    </script>
<script type="text/javascript">
             
        function sbtskSvaeDataowner(val12A) {

            if (Page_ClientValidate("sbtskbtnsaveowner")) {
               
                //debugger


                // save all details


                    $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/sbtskSave_Owner',
                   // data: '{Bindsubtaskdetail: ' + JSON.stringify(Bindsubtaskdetail) + '}',
                    data: "{ 'id': '" + document.getElementById('<%=sbtskhdnchangeid.ClientID%>').value + "' ,'ownerdropdownid': '" + document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessownerchangesbtsk,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessownerchangesbtsk(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=sbtskddlchangeowner]").val("");
                   
                     document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value = "";
                     $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(0);
                    $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
                    document.getElementById("sbtskbtnclsowner").click();
                    document.getElementById("sbtsklblowner_" + document.getElementById("<%=sbtskhdnchnownrghtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function sbtskBinddetails_Owner(RecordId,sbtsklblownerid) {
              
               //debugger;
               document.getElementById("<%=sbtskhdnchnownrghtml.ClientID%>").value = sbtsklblownerid.split('_')[1];
             
                $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskBinddetail_Owner',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1ownersbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1ownersbtsk(response) {
            
            //debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "")
                {
                    document.getElementById('<%=sbtskhdnchangeid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=sbtskhdncahngeowner.ClientID%>').value = Bindsubtaskdetail.find("owner").text();

             <%--  if (Bindsubtaskdetail.find("owner").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
               $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
            }--%>


              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              Multichangeownersubtask();
              if (Bindsubtaskdetail.find("owner").text() != "")
              {
                  //debugger
                  if (Bindsubtaskdetail.find("owner").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text().split(',');
                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("owner").text();
                $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=sbtskddlchangeowner.ClientID%>")).change();
                  }
             
            }


             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>
    
      <%--Owner details bind and update data end  --%>


<%--Team details bind and update data start --%>

<script>
   <%-- //debugger
    var sbtskddLAddteam = $(document.getElementById("<%=sbtskddladdteam.ClientID%>"));
   

    $(sbtskddLAddteam).select2({
        data: [
    <%=sbtskMultiaddteam%>
        ],
        multiple: true,
        placeholder: "Search Team",
        dropdownParent: $("#sbtskaddteam"),
        width: "100%"
    });

   
     $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change(function () {
        //debugger
                                                    var ids = $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val();
        document.getElementById("<%=sbtskhdnaddteam.ClientID %>").value = ids;
        document.getElementById("<%=sbtskddladdteam.ClientID%>").click();

    });--%>



     function sbtskMultiaddteam() {
        //debugger
        var tsk_id = document.getElementById("<%=multichngtaskid.ClientID%>").value;
        
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiChangesubtaskTeam_new',
            data: "{'tsk_id': '" + tsk_id + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var sbtskddladdteam = $(document.getElementById("<%=sbtskddladdteam.ClientID%>"));
                                                         $(sbtskddladdteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             dropdownParent: $("#sbtskaddteam"),
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val();
                                                        document.getElementById("<%=sbtskhdnaddteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=sbtskddladdteam.ClientID%>").click();

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





   
  
    </script>


    <script type="text/javascript">
             
        function sbtskSvaeDatateam(val13A) {

            if (Page_ClientValidate("sbtskbtnsaveteam")) {
               
                //debugger


                // save all details
                

                $.ajax({
                    type: "POST",
                    url: 'MultiProjectTask.aspx/sbtskSave_Team',
                    data: "{ 'id': '" + document.getElementById('<%=sbtskhdnteameid.ClientID%>').value + "' ,'teamdropdownid': '" + document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value + "' }",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    success: OnSuccessTeamsbtsk,
                    error: function (response) {
                        //debugger
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        //debugger
                        alert(response.responseText);
                    }
                    //success: function (response) {
                    //alert("Data Saved successfully.");
                    //window.location.reload();
                    //}
                });
                return false;
                //});
            };


                  
                }
                function OnSuccessTeamsbtsk(response) {
                    //debugger
                    alert("Data Saved successfully."); 
                    $("[id*=sbtskddladdteam]").val("");
                   
                     document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value = "";
                     $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(0);
                    $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
                    document.getElementById("sbtskbtnclsTeam").click();
                    document.getElementById("sbtsklbluser_" + document.getElementById("<%=sbtskhdnaddteamhtml.ClientID%>").value).innerHTML =  response.d;
                   
                    
                }

        

          function sbtskBinddetails_Team(RecordId,sbtsklbluserid) {

          
                //debugger;
               document.getElementById("<%=sbtskhdnaddteamhtml.ClientID%>").value = sbtsklbluserid.split('_')[1];

                $.ajax({
                    url: 'MultiProjectTask.aspx/sbtskBindetail_Team',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1teamsbtsk,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }


         function OnSuccess1teamsbtsk(response) {
            
            //debugger
            try {
                var Bindsubtaskdetail = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (Bindsubtaskdetail.find("id").text() != "") {
                    document.getElementById('<%=sbtskhdnteameid.ClientID%>').value = Bindsubtaskdetail.find("id").text();
                    document.getElementById('<%=sbtskhdnaddteam.ClientID%>').value = Bindsubtaskdetail.find("team").text();

              <%-- if (Bindsubtaskdetail.find("team").text() != "")
             {
               var idsemp = Bindsubtaskdetail.find("team").text().split(',');
               $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
              $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
            }--%>


              document.getElementById('<%=multichngtaskid.ClientID%>').value = Bindsubtaskdetail.find("task").text();
              sbtskMultiaddteam();
              if (Bindsubtaskdetail.find("team").text() != "")
              {
                  //debugger
                  if (Bindsubtaskdetail.find("team").text().includes(','))
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text().split(',');
                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = Bindsubtaskdetail.find("team").text();
                $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=sbtskddladdteam.ClientID%>")).change();
                  }
             
            }


             
                   
                }
            }
            catch
                {
                }

            }


        
        


            </script>

    <%--Team details bind and update data end  --%>

 <%--Multi sub task details End--%>



  <%--sub task save edit delete  start--%>









<script>
   
    function waitagepercentchksubtask(input)
    {
        
        if (input.value > 100)
        {
            
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>








<script>
    if (document.getElementById('<%=Reminder.ClientID%>').value == "0")
    {
            document.getElementById("divremindertype").style.display = "none";
            document.getElementById("divDays").style.display = "none";
            document.getElementById("Divtime").style.display = "none";
            document.getElementById("divNotifyusers").style.display = "none";
            document.getElementById("divChooseDate").style.display = "none";

    }

</script>










<asp:HiddenField ID="hdnsavetypesubtask" runat="server" />
 <%--save data start --%> 

    










<script>
        function deletedatasubtask(strpid,strmid,strtaskid,RecordId,btn) {
            debugger
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                      
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'MultiProjectTask.aspx/Deletesubtaskdata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                   success: function (response)
                    {
//debugger
                    alert('Data Deleted !');                     
                    Bindtask_data();
viewalldetails();
                    //viewsubtask(strpid,strmid,strtaskid);
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

        function OnSuccessdelete_subtaskdata(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>


<%--sub task save edit delete  end--%>



<script> 
    
    <%=setmilstone%>
   
</script>


<%--<script>
    
$(function() {
  $('#showtr2').on('click', function() {
    $('.taskshow2').toggle();
  });
});
$('#showtr2').click(function() {
    $(this).toggleClass('shown');
    });

    //function GetbindtaskdataP_hideshow() {
        //debugger
        var RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
        if (RecordId == "" || RecordId == "0") {
            RecordId = document.getElementById("<%=ProjectId.ClientID%>").value;
        }

        $.ajax({
            url: 'MultiProjectTask.aspx/bindtaskdataP',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: OnSuccesssetmilstonehideshow_data,
            error: function (response) {
                //alert(response.responseText);
                alert(response.responseText);
            },
            failure: function (response) {
                alert(response.responseText);
            }
        });

  //  }

     function OnSuccesssetmilstonehideshow_data(response)
     {
         //debugger
        // // alert(response.d);
        // var todolist = response.d;
        // alert(todolist);
        //// lbl.innerHTML = todolist;
        // todolist;



          //$(function() {$('#showtrM_8').on('click', function() { $('.taskshow_8').toggle();  });}); $('#showtrM_8').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_27').on('click', function() {$('.subtaskshow_27').toggle();  });}); $('#showtasktr_27').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_28').on('click', function() {$('.subtaskshow_28').toggle();  });}); $('#showtasktr_28').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtrM_7037').on('click', function() { $('.taskshow_7037').toggle();  });}); $('#showtrM_7037').click(function() { $(this).toggleClass('shown');}); $(function() {$('#showtasktr_2052').on('click', function() {$('.subtaskshow_2052').toggle();  });}); $('#showtasktr_2052').click(function() { $(this).toggleClass('shown');});
                  
     
        


    }


    


  



</script>--%>



<%--save milestone and edit delete start --%>

<script>
   function Getempdetailsmile() {
        //debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwnermile = $(document.getElementById("<%=Ownermile.ClientID%>"));
                                                         $(ddLOwnermile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=Ownermile.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=Ownermile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnownermile.ClientID %>").value = ids;
                                                        document.getElementById("<%=Ownermile.ClientID%>").click();

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





     function GetTeamempdetailsmile() {
        //debugger
        var RecordId = document.getElementById("<%=hdnProjectIdmile.ClientID%>").value;
       if (RecordId == "" || RecordId == "0") {
           RecordId = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new_mile',
            data: "{'RecordId': '" + RecordId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteammile = $(document.getElementById("<%=teammile.ClientID%>"));
                                                         $(ddLteammile).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=teammile.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=teammile.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteammile.ClientID %>").value = ids;
                                                        document.getElementById("<%=teammile.ClientID%>").click();

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


    function GetWeitagebindDtlsmile() {
        //debugger
                  var RecordId=document.getElementById("<%=ProjectIdmile.ClientID%>").value;

                  $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails_mile',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkmilestone,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });





        function OnSuccessweitagedtlschkmilestone(response) {
            
            //debugger
            try {
                 
                var waitagechkdtlsmile = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtlsmile.find("id").text() != "")
                {
                     // alert(waitagechkdtlsmile.find("checkprojectWeightage").text());
                      if (waitagechkdtlsmile.find("checkprojectWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "none";

                     }

                     if (waitagechkdtlsmile.find("checkprojectWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitagemile").style.display = "block";

                     }
                    
                }
                
            }
            catch

            {

            }

            Getempdetailsmile();
            GetTeamempdetailsmile();

            document.getElementById("divprojecttimeline").style.display = "block";
            Bindprojecttimeline_data();

             document.getElementById("divmiletimeline_mile").style.display = "block";
            Bindmilestonetimeline_data_mile();
          
         }


       
    }
    

</script>



<script>
   
    function waitagepercentchkmile(input)
    {
        
        if (input.value > 100)
        {
            alert('accept only numbers between 1 to 100');
            input.value = 0; 

        }

  }
    

</script>


<script>
    function checkdatemile(enterdate)
    {
        if (document.getElementById("<%=StartDatemile.ClientID%>").value != "" && document.getElementById("<%=EndDatemile.ClientID%>").value != "")
        {
                           const today = new Date(document.getElementById("<%=StartDatemile.ClientID%>").value);
                              var fdt = new Date(today);                             
                              const today2 = new Date(document.getElementById("<%=EndDatemile.ClientID%>").value);
                              var todt = new Date(today2);

                               var t1 = new Date();
                               t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                               var t2 = new Date();
            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
            if (t1 > t2) {
                 alert("Enter Valid Date");
                document.getElementById(enterdate).value = "";
                document.getElementById(enterdate).focus();
                return false;
            }
            else {
return true;
            }
        }
        else
        {
            return true;
        }
    }
    function checkdatecomparisiontaskdate_mile(enterdate) {
        //debugger
        if (enterdate != "") {

            if (checkdatemile(enterdate)) {
                var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(document.getElementById(enterdate).value);
                var date = new Date();

                var fcompdate = new Date();
                //fcompdate = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                fcompdate = new Date(strdt[0], parseInt(strdt[1]) - 1, strdt[2])
                var tcompdate = new Date();
                tcompdate = new Date(date.getFullYear(), date.getMonth(), date.getDate())

                var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;



                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckstrtdateprojectdetails',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        //debugger
                        var milestonestrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (milestonestrtdatechkdtls.find("id").text() != "") {
                            const today = new Date(milestonestrtdatechkdtls.find("StartDate").text());
                            var fdt = new Date(today);
                            const today2 = new Date(milestonestrtdatechkdtls.find("TargetDate").text());
                            var todt = new Date(today2);

                            var t1 = new Date();
                            t1 = new Date(fdt.getFullYear(), fdt.getMonth(), fdt.getDate())
                            var t2 = new Date();
                            t2 = new Date(todt.getFullYear(), todt.getMonth(), todt.getDate())
                            if (fcompdate >= t1 && fcompdate <= t2) {
                                return true
                            }
                            else {
                                alert("Enter Valid Date");
                                document.getElementById(enterdate).value = "";
                                document.getElementById(enterdate).focus();
                                return false;
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });


            }

          
        }
    }

    
    

</script>



<script>
  function GetDurationchange()
  {


    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "0")
    {
        document.getElementById("divchngtype").style.display = "none";
         
    }

    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "1")
    {
        document.getElementById("divchngtype").style.display = "block";
        document.getElementById("chngtype").innerHTML = "(Hours)";
         
    }

    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "2")
    {
        document.getElementById("divchngtype").style.display = "block";
        document.getElementById("chngtype").innerHTML = "(Days)";
          
     }


    if (document.getElementById('<%=ddldurationtype.ClientID%>').value == "3")
    {
        document.getElementById("divchngtype").style.display = "block";
       document.getElementById("chngtype").innerHTML = "(Month)";
        
    }





  } 

</script>



<asp:HiddenField ID="hdnsavetypemile" runat="server" />

    <%--save data start --%> 

    <script type="text/javascript">
                function showdivimgmile() {
                    if (Page_ClientValidate("btnclosemile")) {
                        if (confirm('Do you want Leave this page ?')) {   

                            //Cleardatamile();
                            
                            document.getElementById("btnclosemile").click();
                           
                        }
                        else {
                            return false;
                        }
                        //reloaddata();
                        return true;
                    }
         }


        function AddNewProjectstatusmile()
        {
                //debugger
                document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                 document.getElementById("dvnewprojectstatusmile").style.display = "none";
               if (document.getElementById('<%=P_Statusmile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "1";
                    document.getElementById("dvnewprojectstatusmile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value = "";
                    document.getElementById('<%=stustprintColormile.ClientID%>').value = "";
               }
        }


        function AddNewprioritymile()
        {
                //debugger
                document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                 document.getElementById("dvnewprioritymile").style.display = "none";
               if (document.getElementById('<%=Prioritymile.ClientID%>').value == "-1")
               {
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "1";
                    document.getElementById("dvnewprioritymile").style.display = "block";
                }
               else
               {
                   document.getElementById('<%=txtnewprioritymile.ClientID%>').value = "";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').value = "";
               }
        }

         

      


          function SvaeDatamile(val1Amile) {
                    //debugger
              if (Page_ClientValidate("btnsavemile"))
              {
                       
                       document.getElementById('<%=hdnsavetypemile.ClientID%>').value = val1Amile;  
                        
                        if (confirm('Are You Sure You want to Submit This Entry!')) {
                            document.getElementById("pnlimgmile").style.display = "block";
                            document.getElementById("divsubmile").style.display = "none";

                        }
                        else
                        {
                            return false;
                        }




                      // Project status save
                        
                        var milstonestatusidmile = "0";
                        if (document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "" && document.getElementById('<%=hdnP_Statusmile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value != "" && document.getElementById('<%=stustprintColormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddAddMilestonetatus",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').value) + ',statuscolor: ' + JSON.stringify(document.getElementById('<%=stustprintColormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milstonestatusidmile = response.d;
                                    

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            milstonestatusidmile = document.getElementById('<%=P_Statusmile.ClientID%>').value;
                        }

                        // Priority type save

                         var milePrioritytypeidmile = "0";
                        if (document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "" && document.getElementById('<%=hdnPrioritymile.ClientID%>').value != "0" && document.getElementById('<%=txtnewprioritymile.ClientID%>').value != "" && document.getElementById('<%=txtpriortycolormile.ClientID%>').value != "")
                        {
                            $.ajax({
                            type: "POST",
                            url: "MultiProjectTask.aspx/AddMilestonePrioritytype",
                            data: '{value: ' + JSON.stringify(document.getElementById('<%=txtnewprioritymile.ClientID%>').value) + ',prioritycolor: ' + JSON.stringify(document.getElementById('<%=txtpriortycolormile.ClientID%>').value) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                                success: function (response) {
                                  
                                        milePrioritytypeidmile = response.d;
                                    

                            },
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                                return false;
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                            

                        }
                        else
                        {
                            milePrioritytypeidmile = document.getElementById('<%=Prioritymile.ClientID%>').value;
                        }


                        // save all details

                        
                        var milestonedetails = {};
                        milestonedetails.id = document.getElementById('<%=hdnidmile.ClientID%>').value; 
                        milestonedetails.ProjectId = document.getElementById('<%=ProjectIdmile.ClientID%>').value; 
                        milestonedetails.Milestone = document.getElementById('<%=Milestone.ClientID%>').value; 
                        milestonedetails.Weightage = document.getElementById('<%=Weightagemile.ClientID%>').value; 
                        milestonedetails.StartDate = document.getElementById('<%=StartDatemile.ClientID%>').value; 
                        milestonedetails.EndDate = document.getElementById('<%=EndDatemile.ClientID%>').value; 
                       milestonedetails.Owner = document.getElementById('<%=hdnownermile.ClientID%>').value;
                       milestonedetails.Access = document.getElementById('<%=Accessmile.ClientID%>').value; 
                        milestonedetails.team = document.getElementById('<%=hdnteammile.ClientID%>').value; 
                        milestonedetails.P_Status = milstonestatusidmile;
                        milestonedetails.Priority = milePrioritytypeidmile;
                        milestonedetails.Budget = document.getElementById('<%=Budgetmile.ClientID%>').value;
                        milestonedetails.Actual = document.getElementById('<%=Actualmile.ClientID%>').value;


                       if (document.getElementById('<%=istask.ClientID%>').checked == true)
                       {

                        milestonedetails.istask = 1

                       }

                       if (document.getElementById('<%=istask.ClientID%>').checked == false)
                       {

                        milestonedetails.istask = 0

                       }

                  
                       if (document.getElementById('<%=checkmileWeightage.ClientID%>').checked == true)
                       {

                        milestonedetails.checkmileWeightage = 1

                       }

                       if (document.getElementById('<%=checkmileWeightage.ClientID%>').checked == false)
                       {

                        milestonedetails.checkmileWeightage = 0

                       }


                   milestonedetails.Durationtype = document.getElementById('<%=ddldurationtype.ClientID%>').value; 
                   milestonedetails.TaskDuration = document.getElementById('<%=taskDurationmile.ClientID%>').value; 

                        $.ajax({
                            type: "POST",
                            url: 'MultiProjectTask.aspx/SaveMilestone',
                            data: '{milestonedetails: ' + JSON.stringify(milestonedetails) + '}',
                            contentType: "application/json; charset=utf-8",
                            async: false,
                            dataType: "json",
                            success: OnSuccess_mile,
                            error: function (response) {
                                //debugger
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                //debugger
                                alert(response.responseText);
                            }
                            //success: function (response) {
                            //alert("Data Saved successfully.");
                            //window.location.reload();
                            //}
                        });
                        return false;
                        //});


                    };
                }
                function OnSuccess_mile(response) {
                    //debugger
                    alert("Data Saved successfully."); 

                    document.getElementById('<%=btnloadmile.ClientID%>').click();

                  

                   document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                    document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    
                      document.getElementById('<%=hdnownermile.ClientID%>').value = "";
                     $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                   $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                     $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                    $(document.getElementById("<%=teammile.ClientID%>")).change();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = "0";
                     document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                    document.getElementById('<%=P_Statusmile.ClientID%>').value = "0";
                    document.getElementById('<%=Prioritymile.ClientID%>').value = "0";
                    document.getElementById('<%=Budgetmile.ClientID%>').value = "0";
                    document.getElementById('<%=Actualmile.ClientID%>').value = "0";
                    document.getElementById('<%=istask.ClientID%>').checked = 0;
                    document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;
                    <%--alert(document.getElementById('<%=hdnsavetype.ClientID%>').value);--%>

                    document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";

                     document.getElementById('<%=hdnidmile.ClientID%>').value = "0";
                    if (document.getElementById('<%=hdnsavetypemile.ClientID%>').value == "save")
                    {
                        document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                        document.getElementById("btnclsmile").click();
                        Bindtask_data();
                    }
                    
                    if (document.getElementById('<%=hdnsavetype.ClientID%>').value == "savemore")
                    {
                         document.getElementById("pnlimgmile").style.display = "none";
                        document.getElementById("divsubmile").style.display = "block";
                       // document.getElementById("btncls").click();
                        Bindtask_data();
                    }
                   
                   
               
                    
                }



          function Cleardatamile() {
                        
                  
                 document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = "0";
                  document.getElementById('<%=Milestone.ClientID%>').value = "";
                    document.getElementById('<%=Weightagemile.ClientID%>').value = "";
                    document.getElementById('<%=StartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=EndDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnStartDatemile.ClientID%>').value = "";
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value ="";
                    document.getElementById('<%=hdnownermile.ClientID%>').value ="";
                   document.getElementById('<%=hdnAccessmile.ClientID%>').value = "";
                   document.getElementById('<%=hdnteammile.ClientID%>').value = "";
                   document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = "0";
                   document.getElementById('<%=hdnPrioritymile.ClientID%>').value = "0";
                   document.getElementById('<%=istask.ClientID%>').checked = 0;
                 document.getElementById('<%=checkmileWeightage.ClientID%>').checked = 0;

                   document.getElementById('<%=taskDurationmile.ClientID%>').value = "0";
                    document.getElementById('<%=hdndurationtype.ClientID%>').value = "0";


                  $(document.getElementById("<%=Ownermile.ClientID%>")).val(0);
                 $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  $(document.getElementById("<%=teammile.ClientID%>")).val(0);
                 $(document.getElementById("<%=teammile.ClientID%>")).change();

                   document.getElementById('<%=Button1mile.ClientID%>').click();
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Add Milestone";
         
                        
                    }

        

          function Binddetails_mile(RecordId) {

          
                //debugger;
                 
                $.ajax({
                    url: 'MultiProjectTask.aspx/Bindmilestonedetail',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccess1_milestonedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
              
        }


         function OnSuccess1_milestonedata(response) {
            
            //debugger
            try {
                var milestonedetails = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (milestonedetails.find("id").text() != "") {
                    document.getElementById('<%=hdnidmile.ClientID%>').value = milestonedetails.find("id").text();
                    
                    document.getElementById('<%=ProjectIdmile.ClientID%>').value = (milestonedetails.find("ProjectId").text());

                     document.getElementById('<%=Milestone.ClientID%>').value=(milestonedetails.find("Milestone").text());
                    document.getElementById('<%=Weightagemile.ClientID%>').value=(milestonedetails.find("Weightage").text());
                    document.getElementById('<%=StartDatemile.ClientID%>').value=(milestonedetails.find("StartDate").text());
                    document.getElementById('<%=EndDatemile.ClientID%>').value=(milestonedetails.find("EndDate").text());
                    document.getElementById('<%=hdnownermile.ClientID%>').value=(milestonedetails.find("Owner").text());
                    document.getElementById('<%=Accessmile.ClientID%>').value = (milestonedetails.find("Access").text()); 
                    
                    document.getElementById('<%=Budgetmile.ClientID%>').value = (milestonedetails.find("Budget").text()); 
                    document.getElementById('<%=Actualmile.ClientID%>').value = (milestonedetails.find("Actual").text()); 

                    document.getElementById('<%=P_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                    document.getElementById('<%=hdnP_Statusmile.ClientID%>').value = milestonedetails.find("P_Status").text();
                        document.getElementById('<%=txtnewprojectstatusmile.ClientID%>').style.display = "none";
                    document.getElementById('<%=stustprintColormile.ClientID%>').style.display = "none";


                    document.getElementById('<%=Prioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=hdnPrioritymile.ClientID%>').value = milestonedetails.find("Priority").text();
                    document.getElementById('<%=txtnewprioritymile.ClientID%>').style.display = "none";
                    document.getElementById('<%=txtpriortycolormile.ClientID%>').style.display = "none";

                     document.getElementById('<%=hdnteammile.ClientID%>').value = milestonedetails.find("team").text();

                    document.getElementById('<%=hdnProjectIdmile.ClientID%>').value = milestonedetails.find("ProjectId").text();
                     document.getElementById('<%=hdnStartDatemile.ClientID%>').value = milestonedetails.find("StartDate").text();
                    document.getElementById('<%=hdnEndDatemile.ClientID%>').value = milestonedetails.find("EndDate").text();
                    document.getElementById('<%=hdnAccessmile.ClientID%>').value = milestonedetails.find("Access").text();


                  if (milestonedetails.find("istask").text() == 1)
                  {
                      document.getElementById('<%=istask.ClientID%>').checked = true;

                  }

                  if (milestonedetails.find("istask").text() == 0)
                  {
                      document.getElementById('<%=istask.ClientID%>').checked = false;

                  }


                   if (milestonedetails.find("checkmileWeightage").text() == 1)
                   {
                      document.getElementById('<%=checkmileWeightage.ClientID%>').checked = true;

                   }

                   if (milestonedetails.find("checkmileWeightage").text() == 0)
                   {
                      document.getElementById('<%=checkmileWeightage.ClientID%>').checked = false;

                    }

                    document.getElementById('<%=hdndurationtype.ClientID%>').value = milestonedetails.find("Durationtype").text();
                    document.getElementById('<%=taskDurationmile.ClientID%>').value = milestonedetails.find("TaskDuration").text();

                    
                    if (milestonedetails.find("Durationtype").text() == "1")
                    {
                      document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Hours)";
         
                    }
                    if (milestonedetails.find("Durationtype").text() == "2")
                    {
                     document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Days)";
                    }

                    if (milestonedetails.find("Durationtype").text() == "3")
                    {
                     document.getElementById("divchngtype").style.display = "block";
                     document.getElementById("chngtype").innerHTML = "(Month)";
                    }

                     document.getElementById("divprojecttimeline").style.display = "block";
                    Bindprojecttimeline_data();
                     document.getElementById("divmiletimeline_mile").style.display = "block";
                    Bindmilestonetimeline_data_mile();

                  //  WeitageChecked();
                  //GetWeitagebindDtls();
                    document.getElementById('<%=Button1mile.ClientID%>').click();
                    Getempdetailsmile();

              if (milestonedetails.find("Owner").text() != "")
              {
                  //debugger
                  if (milestonedetails.find("Owner").text().includes(','))
                  {
                      var idsemp = milestonedetails.find("Owner").text().split(',');
                      $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
                      $(document.getElementById("<%=Ownermile.ClientID%>")).change();

                  }
                  else
                  {
                      var idsemp = milestonedetails.find("Owner").text();
                $(document.getElementById("<%=Ownermile.ClientID%>")).val(idsemp);
               $(document.getElementById("<%=Ownermile.ClientID%>")).change();
                  }
             
            }

              GetTeamempdetailsmile();
             if (milestonedetails.find("team").text() != "")
             {
                 //debugger
                 if (milestonedetails.find("team").text().includes(','))
                 {
                      var idsemp = milestonedetails.find("team").text().split(',');
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }
                 else
                 {

                     var idsemp = milestonedetails.find("team").text();
                     $(document.getElementById("<%=teammile.ClientID%>")).val(idsemp);
                     $(document.getElementById("<%=teammile.ClientID%>")).change();
                 }



            }
                  
                     
                    document.getElementById('btnsavemile').value = "Update";
                   document.getElementById('<%=lbltitlemile.ClientID%>').innerHTML= "Edit Milestone";
                }
            }
            catch
                {
                }

            }


        
        


            </script>



<script>
    
    $(document).ready(function ()
    {
            Bindprojecttimeline_data();
});
    function Bindprojecttimeline_data()
    {
             //debugger
            
             var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             //alert(projec_id);
              //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindProjecttimelinedata',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessproject_timeline,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessproject_timeline(response)
        {
            //debugger
            var todolist = response.d;

                    let lbl = document.getElementById("<%=projecttimeline.ClientID%>");
                  //  lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
        }

    </script>
<script>
    
    $(document).ready(function ()
    {
            Bindmilestonetimeline_data_mile();
});
    function Bindmilestonetimeline_data_mile()
    {
             debugger
            
             
             var projec_id = document.getElementById("<%=ProjectIdmile.ClientID%>").value;
             //alert(projec_id);


                //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata_mile',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata_mile,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

        }
        function OnSuccessgetdetails_miletimelinedata_mile(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline_mile.ClientID%>");
                   // lbl.innerHTML = '';                    
                    lbl.innerHTML = todolist;
           
   
           
        }

    </script>







    <script>
        function deletedatamile(strpid,RecordId,btn) {

        //debugger    
                 if (confirm('Do you really want to delete the Entery?'))
                 {
                     //toggleHiddenFieldValue(strpid,RecordId, '','delete') ;
                 }
                  else
                  {
                            return false;
                   }
                 
                    $.ajax({
                    url: 'MultiProjectTask.aspx/DeleteMilestonedata',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response)
                    {
//debugger
                    alert('Data Deleted !');
                     // const row = btn.closest('tr');
                      //row.remove();
                    Bindtask_data();
viewalldetails();
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

        function OnSuccessdeletemile(response) {
            alert('Data Deleted !');
            Bindtask_data();
        }

    </script>




<%--save milestone save and edit delete end--%>

   

<%-- show column hide show start--%>

    <asp:HiddenField ID="showidcolumnid" runat="server" Value="0" />
     
    <script type="text/javascript"> 
        document.querySelectorAll('a.toggle-vis').forEach((el) =>
        {      
            el.addEventListener('click', function (e) {
                    debugger
                    e.preventDefault();

                    let columnIdx = e.target.getAttribute('data-column');
                    let column = table.column(columnIdx);

                    // Toggle the visibility
                    column.visible(!column.visible());

                    var showhide_column = {};
                   // alert(column.visible());
                    showhide_column.id = document.getElementById('<%=showidcolumnid.ClientID%>').value;
                    showhide_column.columnnameid = columnIdx;

                    if (!column.visible()) {
                        showhide_column.isshow = "0";
                      
                    }
                    else {
                        showhide_column.isshow = "1";
                        
                    }
                    
                
                    $.ajax({
                        type: "POST",
                        url: 'MultiProjectTask.aspx/Savecolumndata',
                        data: '{columndetails: ' + JSON.stringify(showhide_column) + '}',
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        dataType: "json",
                        //success: OnSuccess,
                        error: function (response) {
                            //debugger
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            //debugger
                            alert(response.responseText);
                        }
                        //success: function (response) {
                        //alert("Data Saved successfully.");
                        //window.location.reload();
                        //}
                    });
                    return false;
                    
                });
            });
         

    </script>



<script type="text/javascript">
        function hidecolumn() {
            $('.dropdown-item.toggle-vis').click(function (e) {
                if ($(this).hasClass("text-mute")) {
                    $(this).removeClass("text-mute");
                }
                else {
                    $(this).addClass("text-mute");
                }
            });
        }
    </script>

 
      <asp:HiddenField ID="hdn0" runat="server" />
    <asp:HiddenField ID="hdn1" runat="server" />
     <asp:HiddenField ID="hdn2" runat="server" />
     <asp:HiddenField ID="hdn3" runat="server" />
     <asp:HiddenField ID="hdn4" runat="server" />
     <asp:HiddenField ID="hdn5" runat="server" />
     <asp:HiddenField ID="hdn6" runat="server" />
     <asp:HiddenField ID="hdn7" runat="server" />
     <asp:HiddenField ID="hdn8" runat="server" />
     <asp:HiddenField ID="hdn9" runat="server" />
     <asp:HiddenField ID="hdn10" runat="server" />
     <asp:HiddenField ID="hdn11" runat="server" />
    <asp:HiddenField ID="hdn12" runat="server" />
    <asp:HiddenField ID="hdn13" runat="server" />
    <asp:HiddenField ID="hdn14" runat="server" />
    <asp:HiddenField ID="hdn15" runat="server" />
    <asp:HiddenField ID="hdn16" runat="server" />
 
<script>  
       
$(function() {
  $('#showtr').on('click', function() {
    $('.taskshow').toggle();
  });
});
$('#showtr').click(function() {
    $(this).toggleClass('shown');
        });

 


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

   
</script>
    <script type="text/javascript">
function toggleTaskRows(className) {
  const rows = document.querySelectorAll(`.${className}`);
  rows.forEach(row => {
    row.style.display = (row.style.display === 'none' || row.style.display === '') 
      ? 'table-row' 
      : 'none';
  });
        }
        function toggleSubTaskRows(className) {
  const rows = document.querySelectorAll(`.${className}`);
  rows.forEach(row => {
    row.style.display = (row.style.display === 'none' || row.style.display === '') 
      ? 'table-row' 
      : 'none';
  });
}
function showtrnew(strid)
{

var trid="tr_"+strid;
//alert(trid);
document.getElementById("trid").style.display='';
}
function showtrnewsubtask(strid)
{

var trid="tr_"+strid;
//alert(trid);
document.getElementById("trid").style.display='';
}
function showtrnewsubtask(strid)
{

var trid="tr_"+strid;
//alert(trid);
document.getElementById("trid").style.display='';
}
        function hidecolumngrid()
        {
debugger
 var table = $('#myTable').DataTable();
           // alert("yes");
            for (var i = 0; i < 17; i++)
            {  
               // alert("no");
                var aa = "ContentPlaceHolder1_hdn" + i; 
                if (document.getElementById(aa).value == "0")
                { 
                   // alert("0");
                    let column = table.column(i); 
                    column.visible(false);
                    //column.style.display='none';

                }
            }
          
            document.getElementById("divgridview").style.display = "block";
            document.getElementById("divgridview").style.width = "100%";
            document.getElementById("ContentPlaceHolder1_lblprojectdata").style.width = "100%";
           
        }
    </script>

 <%-- show column hide show end--%>



 <%-- task auto fill start --%>
 <script type="text/javascript">
  function AddMultitask(Projectid,Milestoneid) {
      //alert(Milestoneid);
      //alert(Projectid);
      
      //debugger;

      document.getElementById("<%=hdnProjectId.ClientID%>").value = Projectid;
      document.getElementById("<%=hdnMilestoneid.ClientID%>").value = Milestoneid;
      document.getElementById('<%=Button1.ClientID%>').click();
      
        document.getElementById("divproject").style.display = "none";
        document.getElementById("divmilestone").style.display = "none";
        var RecordId=document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
         
         $.ajax({
                    url: 'MultiProjectTask.aspx/Bindcheckweitagedetails',
                    data: "{ 'RecordId': '" + RecordId + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessweitagedtlschkpagewise,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
        });




              function OnSuccessweitagedtlschkpagewise(response) {
            
            //debugger
            try {
                var waitagechkdtls_task = $($.parseXML(response.d)).find("Table");
                var str = "";
                if (waitagechkdtls_task.find("id").text() != "")
                {
                 
                      if (waitagechkdtls_task.find("checkmileWeightage").text() == 1)
                      {
                    
                      document.getElementById("divweitage").style.display = "none";

                     }

                     if (waitagechkdtls_task.find("checkmileWeightage").text() == 0)
                      {
                    
                      document.getElementById("divweitage").style.display = "block";

                     }
                    
                }
            }
            catch

            {

            }


      //Owner Bind Milestone Wise;
        //debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiOwner_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLOwner = $(document.getElementById("<%=owner.ClientID%>"));
                                                         $(ddLOwner).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Owner",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=owner.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=owner.ClientID%>")).val();
                                                        document.getElementById("<%=hdnowner.ClientID %>").value = ids;
                                                        document.getElementById("<%=owner.ClientID%>").click();

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
    





     //Team Bind Milestone Wise;
        //debugger
        var mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       if (mile_stoneid == "" || mile_stoneid == "0") {
           mile_stoneid = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
       }
        $.ajax({
            url: 'MultiProjectTask.aspx/bindMultiTeam_new',
            data: "{'mile_stoneid': '" + mile_stoneid + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                //debugger
                 var tableData = [];
                                                         var jsonString = '['+response.d+']';
 
                                                         tableData = JSON.parse(jsonString);

                                                        var data = $.map(tableData, function (obj) {
                                                          obj.id = obj.id;
                                                          obj.text = obj.text;

                                                          return obj;
                                                        });

                                                         var ddLteam = $(document.getElementById("<%=team.ClientID%>"));
                                                         $(ddLteam).select2({
                                                             data: data,
                                                             multiple: true,
                                                             placeholder: "Search Team",
                                                             width: "100%"
                                                         });
                                                     
                                                      $(document.getElementById("<%=team.ClientID%>")).change(function () {
                                                        //debugger
                                                       var ids = $(document.getElementById("<%=team.ClientID%>")).val();
                                                        document.getElementById("<%=hdnteam.ClientID %>").value = ids;
                                                        document.getElementById("<%=team.ClientID%>").click();

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


        //  Milestone Timeline //
              //debugger
              document.getElementById("divmiletimeline").style.display = "block";
              var mile_id = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
             //alert(projec_id);
               //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindMiletimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccessgetdetails_miletimelinedata,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

     
        function OnSuccessgetdetails_miletimelinedata(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lblmiletimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
        }



           //  Task Timeline //
             //debugger
             document.getElementById("divtasktimeline").style.display = "block";
             var mile_id = document.getElementById("<%=hdnMilestoneid.ClientID%>").value;
             //alert(projec_id);
        
                //debugger;
                  $.ajax({
                      url: 'MultiProjectTask.aspx/bindtasktimelinedata',
                    data: "{ 'mile_id': '" + mile_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: OnSuccesstask_timeline_autofill,
                    error: function (response) {
                        //alert(response.responseText);
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });

      
        function OnSuccesstask_timeline_autofill(response)
        {
            //debugger
            var todolist = response.d;
                    let lbl = document.getElementById("<%=lbltasktimeline.ClientID%>");
                    lbl.innerHTML = todolist;
           
           
        }
    

        }

    </script>
 <%-- task auto fill end --%>

<%--subtask task auto fill start --%>

                     
                </div>
            </div>
      </div>
    </div>

    <script>
        function zoominmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoominmulti").click();
            document.getElementById("zoomoutidmulti").style.display = "block";
            document.getElementById("zoominidmulti").style.display = "none";            
        }
        function zoomoutmulti() {
            window.parent.document.getElementById("btnclosemainpopupzoomoutmulti").click();
            document.getElementById("zoominidmulti").style.display = "block";
             document.getElementById("zoomoutidmulti").style.display="none";            
        }
        function closepopupmainmulti() {
            window.parent.document.getElementById("btnclosemainpopupmulti").click();            
        }
    </script>
   <style>
                    
 .bi-wrapper {
    position: relative;
    display: inline-block;
    cursor: pointer;
}

.bi-tooltip {
    visibility: hidden;
    background-color: #333;
    color: #fff;
    text-align: left;
    padding: 10px;
    border-radius: 5px;
    width: 250px;
    position: absolute;
    z-index: 999;
    bottom: 125%; /* position above the link */
    left: 50%;
    transform: translateX(-50%);
    opacity: 0;
    transition: opacity 0.3s;
    box-shadow: 0 4px 10px rgba(0,0,0,0.3);
    font-size: 14px;
}

.bi-wrapper:hover .custom-tooltip {
    visibility: visible;
    opacity: 1;
}

</style>


  <%--<script>
$(function() {
  $('.timesheet-wrap').daterangepicker({
    opens: 'center',
    locale: {
      format: 'MM/DD/YYYY'
    }
  });
});
</script>--%>
    <script src="/pms/assets/js/dhtmlxgantt.js"></script>
    <link rel="stylesheet" href="/pms/assets/css/dhtmlxgantt_meadow.css">
	<script src="/pms/assets/js/testdatanew.js"></script>
    <div id="dvgantt" style="display:none">
        <div class="offcanvas-body p-0 overflow-hidden">
        <div class="row">
            <div class="col-lg-12" style="display:none">
                <div class="card">
                    <div class="card-header pt-2 pb-2">
                        <div class="row">
                            <div class="col-sm-auto">
                                <div class="inner-head">
                                    <div class="opp-ico orange">
                                        <i class="ri-pencil-ruler-2-line"></i>
                                    </div>
                                    <div class="head-txt">
                                        <p>Project Management</p>                                                    
                                        <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside">
                                            <h2><label id="ContentPlaceHolder1_lblowner">All Projects</label> <i class="bi bi-caret-down-fill"></i></h2>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-end">
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">All Projects</span></a>                                             
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Under Negotiation</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Won</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Scheduled</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Working</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Stuck</span></a>
                                            <a class="dropdown-item fs-sm" href="javascript:void(0);"><span class="align-middle">Done</span></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm mt-1">
                                <div class="d-flex justify-content-sm-end">
                                <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                    <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addproject"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change Owner"><i class="ri-user-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Change team"><i class="ri-team-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span title="Edit" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-edit-line"></i></span></button>
                                    <button type="button" class="btn btn-outline-primary btn-icon"><span title="Delete" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-delete-bin-5-line"></i></span></button>                                        
                                    <button type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span title="View" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover"><i class="ri-grid-fill"></i></span></button>
                                    <div class="dropdown-menu">
                                        <a class="dropdown-item" href="javascript:void(0);" onclick="hidepopup('1');">List</a>
                                        <a class="dropdown-item" href="javascript:void(0);" onclick="hidepopup('2');">Gantt</a>
                                       <%-- <a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/makemilestone.aspx<%=strquery%>')">List</a>
                                        <a class="dropdown-item" href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multinew" onclick="loadsrcnew('inner_popup_multinew','/pms/gantt.aspx<%=strquery%>')">Gantt</a>--%>
                                    </div>
                                </div>
                                </div>
                            </div>                                
                        </div> 
                    </div><!-- end card header -->
                </div>
            </div>


            <div class="col-lg-12">                    
                                        
                        <div id="gantt_here" style='width:100%; height:530px'>
                        </div> 
                   
            </div>
        </div>           
    </div>




<div class="offcanvas offcanvas-end" id="addproject" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">Add Project</h5>
        <div class="hstack gap-2">
            <button type="button" onclick="fsFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" onclick="fseFunction()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
                            <div class="row">
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project ID</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project Name</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Owner</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Status</label>
                                    </div>
                                    <select class="form-control">
                                        <option>-- Select --</option>
                                        <option>Under Negotiation</option>
                                        <option>Won</option>
                                        <option>Scheduled</option>
                                        <option>Working</option>
                                        <option>Stuck</option>
                                        <option>Done</option>
                                    </select>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Project Address</label>
                                    </div>
                                    <div class="row">
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="Address Line 1">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="Address Line 2">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <input type="text" class="form-control" placeholder="City">
                                        </div>
                                        <div class="col-xxl-5 col-lg-5">
                                            <input type="text" class="form-control" placeholder="Zip/Postal Code">
                                        </div>
                                        <div class="col-xxl-7 col-lg-7">
                                            <input type="text" class="form-control" placeholder="State/Province">
                                        </div>
                                        <div class="col-xxl-12 col-lg-12">
                                            <select class="form-control">
                                                <option>-- Select Country --</option>
                                                <option>India</option>
                                                <option>USA</option>
                                                <option>UAE</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Client Name</label>
                                    </div>
                                    <input type="text" class="form-control">
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Date of Award</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Start Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Target Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Approval of Drawing</label>
                                    </div>
                                    <select class="form-control">
                                        <option>Yes</option>
                                        <option>No</option>
                                    </select>
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Drawing Approval Date</label>
                                    </div>
                                    <input type="date" class="form-control">
                                </div>                                
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Upload drawing</label>
                                    </div>
                                    <input type="file" class="form-control">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Save</a>
        <a href="javascript:void(0);" class="btn btn-secondary">Save & Add More</a>
    </div>
</div>
 

<div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>
<div class="offcanvas offcanvas-end" id="addcolumns" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title"">Add Columns</h5>
        <div class="hstack gap-2">
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <div class="offcanvas-body p-0 overflow-hidden">
        <div style="height: calc(100vh - 55px);overflow:auto;">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="add_show_new show_column mb-3">
                            <a class="dropdown-item toggle-vis text-mute" data-column="19"><i class="ri-check-line align-middle fs-lg"></i> Project Address</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="20"><i class="ri-check-line align-middle fs-lg"></i> Client Name</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="21"><i class="ri-check-line align-middle fs-lg"></i> Award Date</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="22"><i class="ri-check-line align-middle fs-lg"></i> Approval of Drawing</a>
                            <a class="dropdown-item toggle-vis text-mute" data-column="23"><i class="ri-check-line align-middle fs-lg"></i> Drawing Approval Date</a>
                        </div>
                        <div class="accordion">
                            <div class="accordion-item text-black" style="border-radius:0px;">
                                <h2 class="accordion-header">
                                    <button class="accordion-button collapsed" style="border-radius:0px;padding:12px;" type="button" data-bs-toggle="collapse"data-bs-target="#AddCustomColumns">+ Add Custom Columns</button>
                                </h2>
                                <div id="AddCustomColumns" class="accordion-collapse collapse">
                                    <div class="lead_form_body">
                                        <div class="row">
                                            <div class="col-xxl-12 col-lg-12">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Column Type</label>
                                                </div>
                                                <select class="form-control">
                                                    <option>-- Select --</option>
                                                    <option selected="selected">Text Box</option>
                                                    <option>Textarea</option>
                                                    <option>Dropdown</option>
                                                    <option>Multi Select</option>
                                                    <option>Date</option>
                                                    <option>Time</option>
                                                    <option>Checkbox</option>
                                                    <option>Radiobox</option>
                                                </select>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Column Name</label>
                                                    <div class="gap-1 flex-shrink-0 align-self-center">
                                                        <div class="form-check">
                                                            <label class="form-label">
                                                                <input class="form-check-input" type="checkbox" />
                                                                Is Mandatory
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <input type="text" class="form-control">
                                            </div>
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Add Dropdown Values</label>
                                                </div>
                                                <div class="bg-light p-3 border">
                                                    <div class="input-group mb-3">
                                                        <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                        <button class="btn btn-success" type="button">Add</button>
                                                    </div>
                                                    <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 1</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 2</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Select 3</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Add Checkbox Values</label>
                                                </div>
                                                <div class="bg-light p-3 border">
                                                    <div class="input-group mb-3">
                                                        <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                        <button class="btn btn-success" type="button">Add</button>
                                                    </div>
                                                    <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Checkbox 1</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Checkbox 2</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12 mb-2" style="display:none;">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Add Radiobox Values</label>
                                                </div>
                                                <div class="bg-light p-3 border">
                                                    <div class="input-group mb-3">
                                                        <input type="text" class="form-control" aria-label="Recipient's username with two button addons">
                                                        <button class="btn btn-success" type="button">Add</button>
                                                    </div>
                                                    <div class="list-group nested-list nested-sortable-handle custom-sortable">
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Radiobox 1</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="list-group-item">
                                                            <div class="d-flex align-items-center">
                                                                <div class="flex-grow-1"><i class="ri-drag-move-fill align-bottom handle"></i>Radiobox 2</div>
                                                                <div class="flex-shrink-0 dropdown">
                                                                    <a href="#!" class="text-reset d-inline-block" data-bs-toggle="dropdown" aria-expanded="false">
                                                                        <i class="bi bi-three-dots-vertical"></i>
                                                                    </a>
                                                                    <ul class="dropdown-menu dropdown-menu-end">
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-pencil-square me-1 align-baseline"></i> Edit</a></li>
                                                                        <li><a class="dropdown-item" href="#"><i class="bi bi-trash3 me-1 align-baseline"></i> Delete</a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Default Value</label>
                                                </div>
                                                <input type="text" class="form-control" style="">
                                                <textarea class="form-control" style="display:none;"></textarea>
                                                <select class="form-control" style="display:none;">
                                                    <option>-- Select --</option>
                                                    <option>Select 1</option>
                                                    <option>Select 2</option>
                                                    <option>Select 3</option>
                                                </select>
                                                <div class="mb-2" style="display:none">
                                                <select id="select_custom" class="form-control" multiple="multiple">
                                                    <option>Select 1</option>
                                                    <option>Select 2</option>
                                                    <option>Select 3</option>
                                                </select>
                                                </div>
                                                <input type="date" class="form-control" style="display:none;">
                                                <input type="time" class="form-control" style="display:none;">
                                        
                                                <div class="form-check" style="display:none;">
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <label class="form-label d-block">
                                                                <input class="form-check-input" type="checkbox" />
                                                                Checkbox 1
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label class="form-label d-block">
                                                                <input class="form-check-input" type="checkbox" />
                                                                Checkbox 2
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-check form-radio-primary" style="display:none;">
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <label class="form-check-label d-block">
                                                                <input value="" name="radiobox" type="radio" class="form-check-input">
                                                                Radiobox 1
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label class="form-check-label d-block">
                                                                <input value="" name="radiobox" type="radio" class="form-check-input">
                                                                Radiobox 2
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Description</label>
                                                </div>
                                                <textarea class="form-control"></textarea>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12">
                                                <div class="d-flex align-items-center">
                                                    <label class="form-label flex-grow-1">Access</label>
                                                </div>
                                                <select id="select_access" class="form-control" multiple="multiple">
                                                    <option>Staff</option>
                                                    <option>Client</option>
                                                </select>
                                            </div>
                                            <div class="col-xxl-12 col-lg-12 mt-3">
                                                <a href="javascript:void(0);" class="btn btn-success">Add</a>
                                            </div>
                                        </div>
                                    </div> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="offcanvas-foorter border p-3 pt-1 pb-1 text-right"> 
        <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-success">Add</a>
    </div>--%>
</div>
<div id="conversation" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Conversation</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pt-0" style="min-height:465px;">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs-custom nav-tab-rpv"
                    role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" data-bs-toggle="tab" href="#Comments1" role="tab">Comments</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#Documents1" role="tab">Documents
                            
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#ActivityLog1" role="tab">Activity Log</a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content text-muted">
                    <div class="tab-pane active" id="Comments1" role="tabpanel">                        
                        <div class="chat-conversation p-3" data-simplebar style="height: 350px;">
                            <ul class="list-unstyled chat-conversation-list chat-sm">
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-2.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Frank Hook</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning All</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list right">
                                    <div class="conversation-list">
                                        <div class="user-chat-content">
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning, How are you? What about our next meeting?</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-1.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Christi</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good morning</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-2.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">
                                            <h6>Frank Hook</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Yeah everything is fine. Our next meeting tomorrow at 10.00 AM</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->

                                <li class="chat-list right">
                                    <div class="conversation-list">
                                        <div class="user-chat-content">
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Wow that's great</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                                <li class="chat-list left">
                                    <div class="conversation-list">
                                        <div class="chat-avatar">
                                            <img src="/pms/assets/images/avatar-1.jpg" alt="">
                                        </div>
                                        <div class="user-chat-content">                                                
                                            <h6>Christi</h6>
                                            <div class="ctext-wrap">
                                                <div class="ctext-wrap-content">
                                                    <p class="mb-0 ctext-content">Good</p>
                                                </div>
                                                <div class="dropdown align-self-start message-box-drop">
                                                    <a class="dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                        <i class="ri-more-2-fill"></i>
                                                    </a>
                                                    <div class="dropdown-menu">
                                                        <a class="dropdown-item" href="#"><i class="ri-reply-line me-2 text-muted align-bottom"></i>Reply</a>
                                                        <a class="dropdown-item" href="#"><i class="ri-file-copy-line me-2 text-muted align-bottom"></i>Copy</a>
                                                        <a class="dropdown-item delete-item" href="#"><i class="ri-delete-bin-5-line me-2 text-muted align-bottom"></i>Delete</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="conversation-name"><small class="text-muted">23/02/2024</small> | <small class="text-muted">09:10 am</small></div>
                                        </div>
                                    </div>
                                </li>
                                <!-- chat-list -->
                            </ul>
                        </div>                        
                        <div class="border-top border-top-dashed">
                            <div class="row g-2 mx-3 mt-2 mb-0">
                                <div class="col">
                                    <div class="position-relative">
                                        <input type="text" class="form-control border-light bg-light" placeholder="Enter Message...">
                                    </div>
                                </div><!-- end col -->
                                <div class="col-auto">
                                    <button type="submit" class="btn btn-secondary"><i class="ti ti-send"></i></button>
                                </div><!-- end col -->
                            </div><!-- end row -->
                        </div>
                    </div>
                    <div class="tab-pane" id="Documents1" role="tabpanel">
                        <%--<div class="lead_form_body p-2 pb-0 mt-2">
                            <input type="text" class="form-control" placeholder="File Name">
                        </div>   --%> 
                        <div class="input-group p-2 mb-2 mt-2">
                            <input type="file" class="form-control" style="border-color:#777;border-radius:0px;" aria-label="Upload">
                            <button class="btn btn-success" style="border-radius:0px;"  type="button">Upload</button>                                                    
                        </div>
                            
                        <div class="p-2 pt-0 pb-0" data-simplebar style="max-height: 300px;">
                            <%--<p>No file selected.</p>--%>
                            <table class="table table-sm text-left">
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-excel-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-image-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-pdf-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-word-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                                <tr>
                                    <td width="45px"><div class="attch_icon"><i class="bi bi-file-earmark-image-fill"></i></div></td>
                                    <td>
                                        <span>File name here...</span><br />
                                        <small class="text-muted">23/02/2024 (79.2 KB)</small>
                                    </td>
                                    <td class="text-right" style="vertical-align:middle;"><a href="javascript:void(0);" class="btn btn-success btn-icon btn-sm"><i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i></a> <a href="javascript:void(0);" class="btn btn-danger btn-icon btn-sm"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="tab-pane" id="ActivityLog1" role="tabpanel">
                        <div class="table-responsive mt-3" data-simplebar style="height: 350px;">
                            <table class="table align-middle table-striped table-hover table-nowrap table-sm">
                                <thead class="table-light">
                                    <tr>
                                        <th align="center" width="30">S. No.</th>
                                        <th align="left">Owner</th>
                                        <th align="left">Project Name</th>
                                        <th align="left">Date & Time</th>
                                        <th align="left">Log Type</th>
                                        <th align="center">Old Value</th>
                                        <th width="15"></th>
                                        <th align="center">New Value</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td align="center">1</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Status</td>
                                        <td align="center"><span class="badge bg-danger text-uppercase new_badge">Stuck</span></td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center"><span class="badge bg-secondary text-uppercase new_badge">Working</span></td>
                                    </tr>
                                    <tr>
                                        <td align="center">2</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Priority</td>
                                        <td align="center"><span class="badge text-secondary bg-secondary-subtle text-uppercase new_badge">Low</span></td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center"><span class="badge text-danger bg-danger-subtle text-uppercase new_badge">High</span></td>
                                    </tr>
                                    <tr>
                                        <td align="center">3</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Budget</td>
                                        <td align="center">-</td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center">$ 5000</td>
                                    </tr>
                                    <tr>
                                        <td align="center">4</td>
                                        <td align="left">Lewis Arnold</td>
                                        <td align="left">Absolute Project</td>
                                        <td align="left">29 Feb, 2024 at 5:00 PM</td>
                                        <td align="left">Target Date</td>
                                        <td align="center">25/02/2024</td>
                                        <td align="center"><i class="bi bi-chevron-right"></i></td>
                                        <td align="center">29/02/2024</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>                                        
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="changeowner" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Change Owner</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <div class="col-sm-12 mb-1">
                            <select class="form-control">
                                <option value="1">Christi</option>
                                <option value="2">Frank Hook</option>
                                <option value="3">Ruby</option>
                                <option value="4">Vivek Sisodia</option>
                                <option value="5">Avneesh</option>
                                <option value="6">Mohit Sharma</option>
                                <option value="7">Amit Kumar</option>
                                <option value="8">Jone Doe</option>
                                <option value="9">Avdhesh Singh</option>
                                <option value="10">Ranjay Varun</option>
                                <option value="11">Sachin Gupta</option>
                                <option value="12">Sanjeev</option>                                
                                <option value="13" selected="selected">Lewis Arnold</option>
                            </select>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Save</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="addteam" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Team</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <div class="col-sm-12 mb-1">
                            <select id="select_emp1" class="form-control" multiple="multiple">
                                <option value="1" selected="selected">Christi</option>
                                <option value="2" selected="selected">Frank Hook</option>
                                <option value="3" selected="selected">Ruby</option>
                                <option value="4" selected="selected">Vivek Sisodia</option>
                                <option value="5" selected="selected">Avneesh</option>
                                <option value="6">Mohit Sharma</option>
                                <option value="7">Amit Kumar</option>
                                <option value="8">Jone Doe</option>
                                <option value="9">Avdhesh Singh</option>
                                <option value="10">Ranjay Varun</option>
                                <option value="11">Sachin Gupta</option>
                                <option value="12">Sanjeev</option>
                            </select>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="boq" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Bill of Quantity (BOQ)</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="row">
                    <div class="col-sm mb-2">
                        <div class="d-flex justify-content-sm-end">
                        <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="IMPORT FROM ANOTHER SOFTWARE"><i class="ri-download-2-line"></i></span></button>
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="SEND BY MAIL"><i class="ri-mail-send-line"></i></span></button>                                           
                            <button type="button" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="DOWNLOAD"><i class="ri-download-line"></i></span></button>
                        </div>
                        </div>
                    </div>                                
                </div>
                <div class="table-responsive">
                    <table class="table align-middle table-striped table-hover table-nowrap table-bordered table-sm">
                        <tr>
                            <th>S.No.</th>
                            <th>Item & Description</th>
                            <th>Quantity</th>
                            <th>Make & Model</th>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<div id="actualbudget" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Project Cost Sheet Actual Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                                              
                    </div>
                </div>
            </div>
            <%--<div class="pb-3 text-center">
                <button type="button" class="btn btn-primary">Add</button>
            </div>--%>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
<script>
	gantt.templates.rightside_text = function (start, end, task) {
		return "ID: #" + task.id;
	};

	gantt.templates.leftside_text = function (start, end, task) {
		return task.duration + " days";
	};




gantt.config.columns = [
  { name: "text", label: "Task Name", width: "250", tree: true, resize: true },
  { name: "start_date", label: "Start Date" , width: "150", align: "center", resize: true },
  {
    name: "end_date",
    label: "End Date",
 width: "150",
    align: "center",
     resize: true 
  },
  { name: "duration", label: "Duration", align: "center", resize: true },
  { name: "add", label: "", width: 44 }
];
gantt.config.grid_resize = true;
gantt.render();

gantt.init("gantt_here");

	//gantt.parse(demo_tasks2);



	 gantt.attachEvent("onGanttRender", function () {
     hideAddButtonsForSubtasks();
     //adddiv('');
});
gantt.attachEvent("onAfterTaskUpdate", function(id, task) {
   //debugger;
   const startDate = task.start_date;
    const endDate = task.end_date;
UpdateTimelineG(id,startDate,endDate);
    

});

</script>



<script>
$('body').on('click', function (e) {
        $('[data-bs-toggle=popover]').each(function () {
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                (($(this).popover('hide').data('bs.popover')||{}).inState||{}).click = false;
            }
        });
    });
</script>

    </div>
    <script>
function hidepopup(strval)
{
if(strval=="1")
{
 document.getElementById('dvgantt').style.display='none';
 document.getElementById('btnsetting').style.display='';
 document.getElementById('add2').style.display='';
 document.getElementById('dvlist').style.display='';
 Bindtask_data();
  try {
    //debugger;

    // Dispose existing tooltips
    const oldTooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    oldTooltips.forEach(function (el) {
        const tooltip = bootstrap.Tooltip.getInstance(el);
        if (tooltip) {
            tooltip.dispose();
        }
    });

    // Select elements with data-bs-toggle and update attributes
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(function (el) {
        el.setAttribute('data-bs-placement', 'top');
        el.setAttribute('data-bs-trigger', 'hover');
    });

    // Initialize new tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl, {
            placement: 'top',
            trigger: 'hover'
        });
    });

} catch (e) {
    console.error('Tooltip init error:', e);
}
}
else if(strval=="2")
{
 document.getElementById('btnsetting').style.display='none';
 document.getElementById('add2').style.display='none';
 document.getElementById('dvlist').style.display='none';
 document.getElementById('dvgantt').style.display='';
 loadGanttData('<%=HttpContext.Current.Request.QueryString["id"].ToString()%>');
 

}
}
    </script>
    <asp:HiddenField ID="hdnnew" runat="server" />
    <asp:HiddenField ID="hdnparentid" runat="server" />
    <asp:HiddenField ID="hdneditid" runat="server" />
    <asp:HiddenField ID="hdnedittype" runat="server" />
                      
        
    <a href="javascript:void(0);" style="display:none" type="button" id="btneditdetails" data-bs-toggle="modal" data-bs-target="#vieweditdetails"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="edit details">+</span></a>
    <div id="vieweditdetails" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
  <%--  <div class="modal-dialog">--%>
        <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h1 class="modal-title" id="lblganttname"></h1>
                <button type="button" id="btncloseG" class="btn-close"  data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                   <%-- <iframe id='editframe' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe> --%>
                    <div class="gantt_cal_larea" style="height:300px;border:0px;">
                        <div class="gantt_cal_lsection">
                            <label>Name</label></div>
                        <div class="gantt_cal_ltext">
                           <input id="txtname" class="form-control" placeholder="Enter Name" required/>
                        </div>
                        <div class="gantt_cal_lsection">
                            <label>Description</label></div>
                        <div class="gantt_cal_ltext" style="height:70px;">
                           <textarea id="txtdescription"  placeholder="Enter Description"></textarea>
                        </div>

                        <div class="gantt_cal_lsection">
                            <label>Time period</label></div>
                        <div style="height:30px;padding-top:0px;font-size:inherit;" class="gantt_section_time">
                            <%--<div class="gantt_time_selects">
                                <select aria-label="Days" id="ddldays"><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select>
                                <select aria-label="Months" id="ddlmonths"><option value="0">January</option><option value="1">February</option><option value="2">March</option><option value="3">April</option><option value="4">May</option><option value="5">June</option><option value="6">July</option><option value="7">August</option><option value="8">September</option><option value="9">October</option><option value="10">November</option><option value="11">December</option></select> 
                                <select aria-label="Years" id="ddlyears">
                                    <option value="2024">2024</option>
                                    <option value="2025">2025</option>
                                    <option value="2026">2026</option>
                                    

                                </select></div> --%>
                          
                                <div class="row">
                                <div class="col-sm-2">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("From Date", "2")%></label>
                                    </div>
                                    

                                    <input type="text"  id="start_dateG" runat="server" class="form-control" onclick="this.showPicker()" onchange="checkdate(this.id)" required>
                                             
                                               
                                </div>
                                    <div class="col-sm-2" id="dvstart">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Start Time", "2")%></label>
                                    </div>
                                    

                                   <input type="text" id="start_timeG" runat="server" class="form-control" onclick="this.showPicker()">        
                                               
                                </div>
                                <div class="col-sm-2">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("To Date", "2")%></label>
                                    </div>
                                    
                                    <input type="text" id="end_dateG" runat="server" class="form-control"  onclick="this.showPicker()" onchange="checkdate(this.id)" required>
                                             
                                </div>
                                    <div class="col-sm-2" id="dvend">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("End Time", "2")%></label>
                                    </div>
                                    

                                   <input type="text" id="end_timeG" runat="server" class="form-control" onclick="this.showPicker()">        
                                               
                                </div>
                                </div>
                            <%--<div class="gantt_duration">
                                <input type="button" class="gantt_duration_dec" value="−">
                                <input type="text" value="5days" class="gantt_duration_value" aria-label="Duration" aria-valuemin="0"><input type="button" class="gantt_duration_inc" value="+"> Days <span>10 November 1900</span>

                            </div>--%></div></div>
                    <div id="pnlimgG" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img4" />
                    </div>
                     <div class="btn-group" id="divsubG" style="display: block;">
                    <a href="javascript:void(0);" role="button" aria-label="Save" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_save_btn_set" id="btnsaveG" onclick="return UpdateDataG();" ><div dhx_button="1" data-dhx-button="1" class="gantt_save_btn"></div><div>Save</div></a>
                   <%-- <div role="button" aria-label="Save" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_save_btn_set"><div dhx_button="1" data-dhx-button="1" class="gantt_save_btn"></div><div>Save</div></div>--%>
                   <%--<div role="button" aria-label="Cancel" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_cancel_btn_set"><div dhx_button="1" data-dhx-button="1" class="gantt_cancel_btn"></div><div>Cancel</div></div>--%>
                         <a href="javascript:void(0);" role="button" aria-label="Delete" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_cancel_btn_set" id="btncancelG" onclick="canceldata();" ><div dhx_button="1" data-dhx-button="1" class="gantt_cancel_btn"></div><div>Cancel</div></a>
                    <%--<div role="button" aria-label="Delete" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_delete_btn_set" style="float:right;"><div dhx_button="1" data-dhx-button="1" class="gantt_delete_btn"></div><div>Delete</div></div>--%>
                    <a href="javascript:void(0);" role="button" aria-label="Delete" tabindex="0" class="gantt_btn_set gantt_left_btn_set gantt_delete_btn_set" style="float:right;" id="btndelG" onclick="return DeleteDataGN();" ><div dhx_button="1" data-dhx-button="1" class="gantt_delete_btn"></div><div>Delete</div></a>
                    </div>
                </div>
            </div>
          
        </div>
    </div>
</div>

    <script>
function checkdatecomparisiontaskdate_subtask(strfromdate,strenddate,strtid) {
        debugger
        let result = false; 
                //var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(strfromdate);
                var edt = new Date(strenddate);
               
               var taski_d = strtid;



                $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckstrtdatetaskdetails',
                    data: "{ 'taski_d': '" + taski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var subtaskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (subtaskstrtdatechkdtls.find("id").text() != "")
                        {
debugger
                            const today = new Date(subtaskstrtdatechkdtls.find("Startdate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(subtaskstrtdatechkdtls.find("Targetdate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
if (fdt<cfdt)
                            {
                             alert("Start date should not to be less then task start date");
                              }
                        else if (fdt>ctodt)
                            {
                             alert("Start date should not to be greater then task end date");
                              }
return false;
                            }
                            else if (edt>ctodt||edt<cfdt)
                            {
                    if (edt>ctodt)
                            {
                             alert("End date should not to be greater then task end date");
                              }
                        else if (edt<cfdt)
                            {
                             alert("End date should not to be leass then task start date");
                              }
                             //alert("Enter Valid Date");
                                 result = false;
                            }
                            else
                            {
                               result = true
                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

           
    }
function checkdatecomparisiontaskdate_subtask_fromdate(strfromdate,strenddate,strtid) {
        debugger
        let result = false; 
                //var strdt = document.getElementById(enterdate).value.split("-");
                var fdt = new Date(strfromdate);
                var edt = new Date(strenddate);               
               var taski_d = strtid;



                $.ajax({
                    url: 'AddSubtask.aspx/Bindcheckstrtdatetaskdetails',
                    data: "{ 'taski_d': '" + taski_d + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var subtaskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (subtaskstrtdatechkdtls.find("id").text() != "")
                        {
debugger
                            const today = new Date(subtaskstrtdatechkdtls.find("Startdate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(subtaskstrtdatechkdtls.find("Targetdate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
                            if (fdt<cfdt)
                            {
                             alert("Start date should not to be less then task start date");
                            }
                            else if (fdt>ctodt)
                            {
                             alert("Start date should not to be greater then task end date");
                            }
                              return false;
                            }                            
                            else
                            {
                               result = true
                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

           
    }
 function checkdatecomparisiontaskdate_task(strfromdate,strenddate,strmid) {
        debugger
      let result = false;  
                var fdt = new Date(strfromdate);
                var edt = new Date(strenddate);
               var milesto_nid = strmid;



                $.ajax({
                    url: 'Add_Task.aspx/Bindcheckstrtdatemiledetails',
                    data: "{ 'milesto_nid': '" + milesto_nid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var taskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (taskstrtdatechkdtls.find("id").text() != "")
                        {
                           
                            const today = new Date(taskstrtdatechkdtls.find("StartDate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(taskstrtdatechkdtls.find("EndDate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
if (fdt<cfdt)
                            {
                             alert("Start date should not to be less then milesone start date");
                              }
                        else if (fdt>ctodt)
                            {
                             alert("Start date should not to be greater then milestone end date");
                              }
                             //alert("Enter Valid Date");
                                 result = false;
                            }
                           else if (edt>ctodt||edt<cfdt)
                            {
if (edt>ctodt)
                            {
                             alert("End date should not to be greater then milestone end date");
                              }
                        else if (edt<cfdt)
                            {
                             alert("End date should not to be leass then milestone start date");
                              }
                             //alert("Enter Valid Date");
                                 result = false;
                            }
                            else
                            {
                               result = true
                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

            
    }
function checkdatecomparisiontaskdate_task_fromdate(strfromdate,strmid) {
        debugger
      let result = false;  
                var fdt = new Date(strfromdate);
               
               var milesto_nid = strmid;



                $.ajax({
                    url: 'Add_Task.aspx/Bindcheckstrtdatemiledetails',
                    data: "{ 'milesto_nid': '" + milesto_nid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                         var taskstrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (taskstrtdatechkdtls.find("id").text() != "")
                        {
                           
                            const today = new Date(taskstrtdatechkdtls.find("StartDate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(taskstrtdatechkdtls.find("EndDate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
                    if (fdt<cfdt)
                            {
                             alert("Start date should not to be less then milesone start date");
                              }
                          else if (fdt>ctodt)
                            {
                             alert("Start date should not to be greater then milestone end date");
                              }
                             //alert("Enter Valid Date");
                                 result = false;
                            }                           
                            else
                            {
                               result = true
                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

            
    }
function checkdatecomparisiontaskdate_mile2(strfromdate,strenddate,strpid) {
        debugger
      let result = false;  
                var fdt = new Date(strfromdate);
                var edt = new Date(strenddate);
               var projec_id = strpid;
               $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckstrtdateprojectdetails',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                        var milestonestrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (milestonestrtdatechkdtls.find("id").text() != "") {
                            debugger
                           const today = new Date(milestonestrtdatechkdtls.find("StartDate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(milestonestrtdatechkdtls.find("TargetDate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
//alert("Enter Valid Date");
                                if (fdt<cfdt)
                                {
                                 alert("Start date should not to be less then project start date?");
                                }
                                else if (fdt>ctodt)
                                 {
                                 alert("Start date should not to be greater then project end date?");
                                }
                                 result = false;
                            }
                           else if (edt>ctodt||edt<cfdt)
                            {
                             //alert("Enter Valid Date");
                                if (edt>ctodt)
                                    {
                                     alert("End date should not to be greater then project end date?");
                                    }
                                    else if (edt<cfdt)
                                     {
                                     alert("End date should not to be less then project start date?");
                                    }
                                 result = false;
                            }
                            else
                            {
                               result = true                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

            
    }
function checkdatecomparisiontaskdate_mile2_fromdate(strfromdate,strpid) {
        debugger
      let result = false;  
                var fdt = new Date(strfromdate);
               
               var projec_id = strpid;
               $.ajax({
                    url: 'Addmilestone.aspx/Bindcheckstrtdateprojectdetails',
                    data: "{ 'projec_id': '" + projec_id + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger
                        var milestonestrtdatechkdtls = $($.parseXML(response.d)).find("Table");
                        var str = "";
                        if (milestonestrtdatechkdtls.find("id").text() != "") {
                            debugger
                           const today = new Date(milestonestrtdatechkdtls.find("StartDate").text());
                            var cfdt = new Date(today);
                            const today2 = new Date(milestonestrtdatechkdtls.find("TargetDate").text());
                            var ctodt = new Date(today2);

                            if (fdt<cfdt||fdt>ctodt)
                            {
//alert("Enter Valid Date");
                                if (fdt<cfdt)
                                {
                                 alert("Start date should not to be less then project start date?");
                                }
                                else if (fdt>ctodt)
                                 {
                                 alert("Start date should not to be greater then project end date?");
                                }
                                 result = false;
                            }                          
                            else
                            {
                               result = true                               
                            }

                        }

                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
return result;

            
    }

    </script>

     <%--<script src="/pms/assets/js/pmscal_1_13_3_jquery_ui.js"></script>
    <link rel="stylesheet" href="/pms/assets/css/pmscal.css">
<script src="/pms/assets/js/pmscal_lib_1_14_16_jquery_mask_min.js"></script>--%>
    <asp:HiddenField ID="hdnnewdate" runat="server" />
<script src="https://code.jquery.com/ui/1.13.3/jquery-ui.js"></script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.3/themes/base/jquery-ui.css">

 <script>
   $(function() {
    $(".datepicker").datepicker({
        dateFormat: "dd/mm/yy",
        duration: "fast",
         yearRange: "1900:2100",
        changeMonth: true,
        changeYear: true,
         onSelect: function(dateText, inst) {
            // Update the input value manually for the mask
            $(this).val(dateText);
            $(this).trigger('blur');
            $(this).focus();

            // Trigger onchange immediately
             
        }

    });
});
</script>
    <script>
//$('.datepicker').mask('00/00/0000');
</script>
   <script>
function settimelinenewww(strid) {
    debugger;

    let isValid = true;
    if (!strid) return;
    var textval = document.getElementById(strid).value; 
    var ids = strid.slice(0, -2).split("_");
let pid = "0", mid = "0", tid = "0", stid = "0";
if (ids.length > 1) {
        pid = ids[1] || "0";
        mid = ids[2] || "0";
        tid = ids[3] || "0";
        stid = ids[4] || "0";
    }
else
{
return;
}
if (stid !== "0") {
        isValid = checkdatecomparisiontaskdate_subtask_fromdate(makedatepart(textval), tid);
    } else if (tid !== "0") {
        isValid = checkdatecomparisiontaskdate_task_fromdate(makedatepart(textval), mid);
    } else if (mid !== "0") {
        isValid = checkdatecomparisiontaskdate_mile2_fromdate(makedatepart(textval), pid);
    }
if (!isValid) 
{
//document.getElementById(strid).value="";
//document.getElementById(strid).focus();
return;
}
    
var ele=pid;
if(mid!="0")
{
ele+='_'+mid;
}
if(tid!="0")
{
ele+='_'+tid;
}
if(stid!="0")
{
ele+='_'+stid;
}
var daysvalue = document.getElementById('txtday_'+ele+'_6').value;
//alert(daysvalue);
 //return;
$.ajax({
        url: 'makemilestone.aspx/updatetimelinenew',
        data: JSON.stringify({
            upid:pid,
            umid:mid,
            utid:tid,
            ustid:stid,
            adddate:textval,
            adddays:daysvalue,
            
        }),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",

        success:function(response){
        if(response.d!="")
        {
        debugger
            var resp=response.d.split("^");
          if(resp.length==3)
             {

// Calculate date

       var selectedfdate= resp[1].trim();
       var selectedtdate= resp[2].trim();
       var strdateFV=formatDateNew(selectedfdate);
      var strdateTV=formatDateNew(selectedtdate);
      var daysno=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate) ,'ndays')+1;
      var days=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate),'days');
       var strdays="";
        if(days<0)
        {
        strdays= -((days+1))+" days delayed";
        }
        else if((days-1)>0)
        {
        strdays= days+" days left";
        }


//alert(strdays);
if (typeof resp === "undefined" || resp.length < 2) {
        console.error("selectedValue is missing");
        return;
    }
    let fromDate = resp[1].trim();
    let toDate = resp[2].trim();
 if (stid !== "0") {
        isValid = checkdatecomparisiontaskdate_subtask(fromDate, toDate, tid);
    } else if (tid !== "0") {
        isValid = checkdatecomparisiontaskdate_task(fromDate, toDate, mid);
    } else if (mid !== "0") {
        isValid = checkdatecomparisiontaskdate_mile2(fromDate, toDate, pid);
    }
if (!isValid) return;
 let suffix = pid;
    if (mid !== "0") suffix += "_" + mid;
    if (tid !== "0") suffix += "_" + tid;
    if (stid !== "0") suffix += "_" + stid;

    const strtitle = document.querySelector('#title1_' + suffix);
    const strspan1 = document.querySelector('#spn1_' + suffix);
    const strspan2 = document.querySelector('#spn2_' + suffix);

    if (strtitle) {
        strtitle.title = strdays;
        strtitle.setAttribute('data-bs-original-title', strdays);
    }
let fromDateOnly = viewdateformat(fromDate);
let toDateOnly = viewdateformat(toDate);

if (strspan1) {
    strspan1.textContent = fromDateOnly + " - " + toDateOnly;
}
    //if (strspan1) {
     //   strspan1.textContent = strdateFV + "-" + strdateTV;
    //}

    if (strspan2) {
        strspan2.textContent = daysno + " days";
    }
// end cal date
debugger
//document.getElementById(strid).value=viewdateformatnew(fromDate);

            }
        }
},
        error: function (response) {
debugger
        //document.getElementById("pnlimg").style.display='none';
            alert(response.responseText);
        }
    });


   
}
    </script>
    <script>
function settimeline(strid) {
    debugger;

    let isValid = true;

    if (!strid) return;
    var textval = document.getElementById(strid).value;  

    var ids = strid.slice(0, -2).split("_");

//txtday_1_1034_6
    let pid = "0", mid = "0", tid = "0", stid = "0";

    if (ids.length > 1) {
        pid = ids[1] || "0";
        mid = ids[2] || "0";
        tid = ids[3] || "0";
        stid = ids[4] || "0";
    }
var ele=pid;
if(mid!="0")
{
ele+='_'+mid;
}
if(tid!="0")
{
ele+='_'+tid;
}
if(stid!="0")
{
ele+='_'+stid;
}
var daysvalue = document.getElementById('txtday_'+ele+'_6').value;
//alert(daysvalue);
 //return;
$.ajax({
        url: 'makemilestone.aspx/updatetimelinenew',
        data: JSON.stringify({
            upid:pid,
            umid:mid,
            utid:tid,
            ustid:stid,
            adddate:textval,
            adddays:daysvalue,
            
        }),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",

        success:function(response){
        if(response.d!="")
        {
        debugger
            var resp=response.d.split("^");
          if(resp.length==3)
             {

// Calculate date

       var selectedfdate= resp[1].trim();
       var selectedtdate= resp[2].trim();
       var strdateFV=formatDateNew(selectedfdate);
      var strdateTV=formatDateNew(selectedtdate);
      var daysno=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate) ,'ndays')+1;
      var days=formatDateDiff(makedatepart(selectedfdate),makedatepart(selectedtdate),'days');
       var strdays="";
        if(days<0)
        {
        strdays= -((days+1))+" days delayed";
        }
        else if((days-1)>0)
        {
        strdays= days+" days left";
        }


//alert(strdays);
if (typeof resp === "undefined" || resp.length < 2) {
        console.error("selectedValue is missing");
        return;
    }
 let fromDate = resp[1].trim();
    let toDate = resp[2].trim();
 //if (stid !== "0") {
   //     isValid = checkdatecomparisiontaskdate_subtask(fromDate, toDate, tid);
   // } else if (tid !== "0") {
    //    isValid = checkdatecomparisiontaskdate_task(fromDate, toDate, mid);
    //} else if (mid !== "0") {
    //    isValid = checkdatecomparisiontaskdate_mile2(fromDate, toDate, pid);
    //}
//if (!isValid) return;
 let suffix = pid;
    if (mid !== "0") suffix += "_" + mid;
    if (tid !== "0") suffix += "_" + tid;
    if (stid !== "0") suffix += "_" + stid;

    const strtitle = document.querySelector('#title1_' + suffix);
    const strspan1 = document.querySelector('#spn1_' + suffix);
    const strspan2 = document.querySelector('#spn2_' + suffix);

    if (strtitle) {
        strtitle.title = strdays;
        strtitle.setAttribute('data-bs-original-title', strdays);
    }
let fromDateOnly = viewdateformat(fromDate);
let toDateOnly = viewdateformat(toDate);

if (strspan1) {
    strspan1.textContent = fromDateOnly + " - " + toDateOnly;
}
    //if (strspan1) {
     //   strspan1.textContent = strdateFV + "-" + strdateTV;
    //}

    if (strspan2) {
        strspan2.textContent = daysno + " days";
    }
// end cal date
debugger
//document.getElementById(strid).value=viewdateformatnew(fromDate);
//document.getElementById(strid).focus();

            }
        }
},
        error: function (response) {
debugger
        //document.getElementById("pnlimg").style.display='none';
            alert(response.responseText);
        }
    });


   
}
    </script>
    
</asp:Content>



