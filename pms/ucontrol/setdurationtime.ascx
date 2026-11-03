<%@ Control Language="C#" AutoEventWireup="true" CodeFile="setdurationtime.ascx.cs" Inherits="setdurationtime" %>


    <script>
 
 $(document).ready(function () {
    bindemployee_data();
});

function bindemployee_data() {

    $.ajax({
        url: 'setdurationtime.aspx/binddata', // ✅ call ASPX, not ASCX
        data: '{}',
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",

        success: function (response) {

            if (response.d == "1") {
                $('#checkbox').prop('checked', true);
            } else {
                $('#checkbox').prop('checked', false);
            }
        },

        error: function (response) {
            alert(response.responseText);
        }
    });
}

function updateaccessdetails(control) {
    debugger;

    var isChecked = control.checked;

    $.ajax({
        url: '/pms/setdurationtime.ascx/UpdateAccess',
        type: "POST",
        data: JSON.stringify({ isChecked: isChecked }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function(response) {
            if (response.d === "Success") {
                alert("Updated successfully");
            } else {
                alert("Error: " + response.d);
            }
        },

        error: function(xhr, status, error) {
            //alert("AJAX Error: " + error);
        }
    });
}


</script>
<div class="form-check form-switch form-switch-md form-switch-success" dir="ltr">
   On working days? <input type="checkbox" 
           class="form-check-input" 
           id="chkform_1"
           onclick="updateaccessdetails(this)" />
</div>
                            <asp:HiddenField ID="hdnself" runat="server" value="0"/>
                        



