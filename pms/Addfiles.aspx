<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="Addfiles.aspx.cs" Inherits="Addfiles" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
   <style>
        .hierarchy-line {
            padding: 2px;
            margin-bottom: 1px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .complete {
            background-color: #28a745; /* Green */
            color: white;
        }

        .sub-complete span {
            background-color: #d4edda; /* Light green for sub-complete parts */
            padding: 3px 6px;
            border-radius: 3px;
        }

        .label {
            margin-right: 2px;
        }

#dropZone {border:2px dashed #ccc;padding:20px;text-align:center;border-radius:10px;cursor:pointer;transition: 0.3s;}
    </style>
    
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    
    
   
       <%-- <div data-simplebar style="height: calc(100vh - 57px);">  --%>
            
            
                     <div class="lead_form_body p-0 pb-0">
                         <div class="input-group p-0 mb-0" style="display:none;">
                             
                                                    <asp:FileUpload ID="fileupload" CssClass="form-control" runat="server" OnChange="UploadFiles();" multiple /> 
                                                    <asp:Button ID="btnuploadimg" runat="server" Text="Upload"  OnClientClick="UploadFiles();"  /> 
                             
                                                    
                                </div>
                         <div id="dropZone" ><i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
    Drag & Drop files here or click to upload
</div>
                          <a href="#" id="imgpath" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>
                         <div id="previewContainer" style="margin-top:10px;"></div>
                         <asp:HiddenField ID="hdntype" runat="server" />
                         <asp:HiddenField ID="hdnpid" runat="server" />
                         <asp:HiddenField ID="hdnfid" runat="server" />

                          <asp:HiddenField ID="hdnstid" runat="server" />
                          <asp:HiddenField ID="hdntid" runat="server" />
                          <asp:HiddenField ID="hdnmid" runat="server" />
                          <asp:HiddenField ID="hdncallfrom" runat="server" />
                          <asp:HiddenField ID="hdncountid" runat="server" />
                    </div> 
                
                 <div id="pnlimg" style="text-align:center;display:none;">
                        <img src="/pms/Image/loading8.gif" runat="server"/>
                    </div>
    <div style="max-height:300px;overflow-y:auto;">
                <asp:Label ID="lblfilesdetail" runat="server" Text=""></asp:Label>
      </div>
   
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
 $(document).ready(function () {
            

            Bindtask_data();

          
           
});
  function UploadFiles() {
debugger
    var fileUpload = document.getElementById("<%=fileupload.ClientID%>");

    if (fileUpload.files.length > 0) {

        let files = fileUpload.files;

        var ftype = document.getElementById("<%=hdntype.ClientID%>").value;
        var fpid = document.getElementById("<%=hdnpid.ClientID%>").value;
        var fid = document.getElementById("<%=hdnfid.ClientID%>").value;
        let data = new FormData();
        for (let i = 0; i < files.length; i++) {
            data.append("files", files[i]);
        }

        $.ajax({
            url: "SingleFileUploadHandler.ashx?pid=" + fpid + "&ftype=" + ftype + "&id=" + fid,
            type: "POST",
            data: data,
            contentType: false,
            processData: false,

            success: OnSuccessFiles,

            error: function (err) {
                alert("error");
            }
        });
    }
}
        function OnSuccessFiles(response) {
            document.getElementById("<%=fileupload.ClientID%>").value = null;
            Bindtask_data();

         debugger
        }
 function Bindtask_data() {
debugger
document.getElementById("pnlimg").style.display='';
    $.ajax({
        url: 'addfiles.aspx/bindfiledata',
        data: JSON.stringify({
            pid:document.getElementById("<%=hdnpid.ClientID%>").value,
            id:document.getElementById("<%=hdnfid.ClientID%>").value,
            type:document.getElementById("<%=hdntype.ClientID%>").value
        }),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",

        success: OnSuccessgetdetailsmain,
        error: function (response) {
        document.getElementById("pnlimg").style.display='none';
            alert(response.responseText);
        }
    });
}
   function OnSuccessgetdetailsmain(response) {
    debugger;

    var todolist = response.d.split("^");

    try {
        var lblid = document.getElementById("<%=hdncountid.ClientID%>").value;
        window.parent.document.getElementById(lblid).innerHTML = todolist[0];
    } catch {
    }

    try {
        let lbl = document.getElementById("<%=lblfilesdetail.ClientID%>");
        lbl.innerHTML = todolist[1];
    } catch {
    }

    document.getElementById("pnlimg").style.display = 'none';
}
function ShowPreview() {
    debugger;

    var input = document.getElementById("<%= fileupload.ClientID %>");
    var previewContainer = document.getElementById("previewContainer");

    // Clear previous previews
    previewContainer.innerHTML = '';

    // Horizontal layout
    previewContainer.style.display = 'flex';
    previewContainer.style.flexDirection = 'row';
    previewContainer.style.flexWrap = 'wrap';
    previewContainer.style.gap = '8px';

    if (input.files && input.files.length > 0) {
        let isAllValid = true;

        // Validate files
        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];
            var isImage = file.type.startsWith('image/');
            var isPDF = file.type === 'application/pdf';

            if (!isImage && !isPDF) {
                alert("Only image or PDF files are allowed.");
                isAllValid = false;
                break;
            }

            if (isImage && file.size > 25000) { // 25KB limit
                alert("Image '" + file.name + "' exceeds 25KB.");
                isAllValid = false;
                break;
            }

            if (isPDF && file.size > 1048576) { // 1MB limit
                alert("PDF '" + file.name + "' exceeds 1MB.");
                isAllValid = false;
                break;
            }
        }

        if (!isAllValid) {
            input.value = "";
            previewContainer.innerHTML = '';
            return;
        }

        // Preview files
        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i];
            var reader = new FileReader();

            reader.onload = (function (theFile) {
                return function (e) {
                    var wrapper = document.createElement("div");
                    wrapper.style.width = "60px";   // small size
                    wrapper.style.height = "60px";
                    wrapper.style.borderRadius = "8px"; // rounded corners
                    wrapper.style.overflow = "hidden";
                    wrapper.style.display = "flex";
                    wrapper.style.alignItems = "center";
                    wrapper.style.justifyContent = "center";
                    wrapper.style.backgroundColor = "#f5f5f5"; // light background
                    wrapper.title = theFile.name;

                    if (theFile.type.startsWith('image/')) {
                        var img = document.createElement("img");
                        img.src = e.target.result;
                        img.style.width = "100%";
                        img.style.height = "100%";
                        img.style.objectFit = "cover"; // fill wrapper, keep aspect
                        wrapper.appendChild(img);
                    } else if (theFile.type === 'application/pdf') {
                        var icon = document.createElement("div");
                        icon.textContent = "📄";
                        icon.style.fontSize = "24px";
                        wrapper.appendChild(icon);
                    }

                    previewContainer.appendChild(wrapper);
                };
            })(file);

            reader.readAsDataURL(file);
        }
    }
}

 function removedocumentnew(strid) {
                debugger      
            
                  var strconfirm = "Do You Really Want To Delete ?";
                // if (confirm(strconfirm)) {
                     $.ajax({
                         url: "/pms/Deleteimagehandler.ashx?id=" + strid,
                         type: "POST",
                         contentType: false,
                         processData: false,
                         async: false,
                         success: function (response) {
                             debugger
                             //document.getElementById(strid).value = null;
                             if (response == "1") {
                                // alert("delete successfully");
                                Bindtask_data();
                                 
                             }
                             else {
                                 alert(response);
                             }

                         },

                         error: function (err) {
                             // alert(err.statusText)  
                         }
                     });
                 //}
             
             
         }
    </script>
   <script>
const dropZone = document.getElementById("dropZone");
const fileInput = document.getElementById("<%= fileupload.ClientID %>");

// Click → open file picker
dropZone.addEventListener("click", () => fileInput.click());

// Highlight on drag
dropZone.addEventListener("dragover", (e) => {
    e.preventDefault();
    dropZone.style.borderColor = "#007bff";
    dropZone.style.backgroundColor = "#f0f8ff";
});

// Remove highlight
dropZone.addEventListener("dragleave", () => {
    dropZone.style.borderColor = "#ccc";
    dropZone.style.backgroundColor = "";
});

// Drop files
dropZone.addEventListener("drop", (e) => {
    e.preventDefault();

    dropZone.style.borderColor = "#ccc";
    dropZone.style.backgroundColor = "";

    const files = e.dataTransfer.files;

    if (files.length > 0) {

        // Assign files to input
        fileInput.files = files;

        // Show preview (reuse your function)
        //ShowPreview();

        // Optional: auto upload
         UploadFiles();
    }
});
   </script>
</asp:Content>
